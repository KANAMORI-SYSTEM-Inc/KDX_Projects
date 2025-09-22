using Kdx.Contracts.DTOs;
using Kdx.Contracts.Enums;
using Kdx.Contracts.Interfaces;
using System.Diagnostics;

namespace Kdx.Infrastructure.Services
{
    /// <summary>
    /// ProsTime（工程時間）デバイスの管理サービス実装
    /// Infrastructure層に配置することで、ビジネスロジックをUI層から分離
    /// </summary>
    public class ProsTimeDeviceService : IProsTimeDeviceService
    {
        private readonly IAccessRepository _repository;
        private readonly Dictionary<int, OperationProsTimeConfig> _loadedOperationConfigs;

        public class OperationProsTimeConfig
        {
            public int TotalProsTimeCount { get; set; }
            public Dictionary<int, int> SortIdToCategoryIdMap { get; set; }
            public OperationProsTimeConfig()
            {
                SortIdToCategoryIdMap = new Dictionary<int, int>();
            }
        }

        // デフォルト設定
        private static readonly OperationProsTimeConfig _defaultOperationConfig =
            new() { TotalProsTimeCount = 0, SortIdToCategoryIdMap = new Dictionary<int, int>() };

        public ProsTimeDeviceService(IAccessRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _loadedOperationConfigs = LoadOperationProsTimeConfigsFromDb();
        }

        /// <summary>
        /// ProsTimeテーブルの全レコードを削除する
        /// </summary>
        public void DeleteProsTimeTable()
        {
            _repository.DeleteProsTimeTable();
        }

        public List<ProsTime> GetProsTimeByPlcId(int plcId)
        {
            return _repository.GetProsTimeByPlcId(plcId);
        }

        public List<ProsTime> GetProsTimeByMnemonicId(int plcId, int mnemonicId)
        {
            return _repository.GetProsTimeByMnemonicId(plcId, mnemonicId);
        }

        private Dictionary<int, OperationProsTimeConfig> LoadOperationProsTimeConfigsFromDb()
        {
            var configs = new Dictionary<int, OperationProsTimeConfig>();

            try
            {
                var rawConfigData = _repository.GetProsTimeDefinitions();

                if (rawConfigData == null || !rawConfigData.Any())
                {
                    // ProsTimeDefinitionsテーブルから設定を読み込めませんでした
                    return configs; // 空のコンフィグを返す
                }

                var groupedData = rawConfigData.GroupBy(r => r.OperationCategoryId);

                foreach (var group in groupedData)
                {
                    var operationCategoryKey = group.Key;
                    var totalCount = group.First().TotalCount;

                    var map = new Dictionary<int, int>();
                    foreach (var item in group)
                    {
                        map[item.SortOrder] = item.OperationDefinitionsId;
                    }

                    configs[operationCategoryKey] = new OperationProsTimeConfig
                    {
                        TotalProsTimeCount = totalCount,
                        SortIdToCategoryIdMap = map
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading ProsTime definitions: {ex.Message}");
                return GetDefaultConfigs();
            }

            if (!configs.Any())
            {
                return GetDefaultConfigs();
            }

            return configs;
        }

        private Dictionary<int, OperationProsTimeConfig> GetDefaultConfigs()
        {
            var configs = new Dictionary<int, OperationProsTimeConfig>();

            // 各CategoryIdに対してデフォルト設定を生成（5個のProsTime）
            for (int categoryId = 1; categoryId <= 20; categoryId++)
            {
                configs[categoryId] = new OperationProsTimeConfig
                {
                    TotalProsTimeCount = 5,
                    SortIdToCategoryIdMap = new Dictionary<int, int>
                    {
                        {0, 1}, {1, 2}, {2, 3}, {3, 4}, {4, 5}
                    }
                };
            }

            return configs;
        }

        public void SaveProsTime(List<Operation> operations, int startCurrent, int startPrevious, int startCylinder, int plcId)
        {
            operations.Sort((o1, o2) => o1.Id.CompareTo(o2.Id));

            var allExistingProsTimesRaw = GetProsTimeByMnemonicId(plcId, (int)MnemonicType.Operation);
            var existingProsTimeMap = allExistingProsTimesRaw
                .GroupBy(pt => pt.RecordId)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(pt => pt.SortId, pt => pt)
                );

            var difinitions = _repository.GetDifinitions("Operation");
            var cylinders = _repository.GetCYs().Where(c => c.PlcId == plcId).ToList();

            var prosTimesToSave = new List<ProsTime>();
            int count = 0;
            List<Memory> currentMemories = new List<Memory>();
            List<Memory> previousMemories = new List<Memory>();
            List<Memory> cylinderMemories = new List<Memory>();


            foreach (Operation operation in operations)
            {
                if (operation == null || operation.CategoryId == null) continue;

                var operationCategoryValue = operation.CategoryId.Value;

                OperationProsTimeConfig currentConfig = _loadedOperationConfigs.TryGetValue(operationCategoryValue, out var specificConfig)
                                                        ? specificConfig
                                                        : _defaultOperationConfig;

                int prosTimeCount = currentConfig.TotalProsTimeCount;

                for (int i = 0; i < prosTimeCount; i++)
                {
                    string currentDevice = "ZR" + (startCurrent + count).ToString();
                    string previousDevice = "ZR" + (startPrevious + count).ToString();
                    string cylinderDevice = "ZR" + (startCylinder + count).ToString();

                    ProsTime? existing = null;
                    if (existingProsTimeMap.TryGetValue(operation.Id, out var opGroup) &&
                        opGroup.TryGetValue(i, out var foundProsTime))
                    {
                        existing = foundProsTime;
                    }

                    count++;
                    ProsTime prosTime = new ProsTime
                    {
                        PlcId = plcId,
                        MnemonicId = (int)MnemonicType.Operation,
                        RecordId = operation.Id,
                        SortId = i,
                        CurrentDevice = currentDevice,
                        PreviousDevice = previousDevice,
                        CylinderDevice = cylinderDevice,
                        CategoryId = currentConfig.SortIdToCategoryIdMap.TryGetValue(i, out var catId) ? catId : 0
                    };

                    var difinition = difinitions.FirstOrDefault(d => d.OutCoilNumber == operation.Id);
                    var cylinder = cylinders.FirstOrDefault(c => c.Id == operation.CYId);
                    string row2 = cylinder != null ? cylinder.CYNum ?? "NaN" : "NaN";
                    string row3 = difinition != null ? difinition.Comment1 ?? "" : "";
                    string row4 = difinition != null ? difinition.Comment2 ?? "" : "";

                    Memory currentMemory = new()
                    {
                        PlcId = plcId,
                        Device = currentDevice,
                        MemoryCategory = 5,     // ZR
                        DeviceNumber = startCurrent + count,
                        DeviceNumber1 = (startPrevious + count).ToString(),
                        DeviceNumber2 = "",
                        Category = "工程ﾀｲﾑ",
                        Row_1 = "ﾀｲﾑ現在",
                        Row_2 = row2,
                        Row_3 = row3,
                        Row_4 = row4,
                        Direct_Input = "",
                        Confirm = "",
                        Note = "",
                        GOT = "true",
                        MnemonicId = (int)MnemonicType.Operation,
                        RecordId = operation.Id,
                        OutcoilNumber = 0
                    };

                    Memory previousMemory = new()
                    {
                        PlcId = plcId,
                        Device = previousDevice,
                        MemoryCategory = 5,     // ZR
                        DeviceNumber = startPrevious + count,
                        DeviceNumber1 = (startCurrent + count).ToString(),
                        DeviceNumber2 = "",
                        Category = "前工程ﾀｲﾑ",
                        Row_1 = "ﾀｲﾑ前回",
                        Row_2 = row2,
                        Row_3 = row3,
                        Row_4 = row4,
                        Direct_Input = "",
                        Confirm = "",
                        Note = "",
                        GOT = "true",
                        MnemonicId = (int)MnemonicType.Operation,
                        RecordId = operation.Id,
                        OutcoilNumber = 0
                    };

                    Memory cylinderMemory = new()
                    {
                        PlcId = plcId,
                        Device = cylinderDevice,
                        MemoryCategory = 5,     // ZR
                        DeviceNumber = startCylinder + count,
                        DeviceNumber1 = "",
                        DeviceNumber2 = "",
                        Category = "ｼﾘﾝﾀﾞ",
                        Row_1 = "CYﾀｲﾑ",
                        Row_2 = row2,
                        Row_3 = row3,
                        Row_4 = row4,
                        Direct_Input = "",
                        Confirm = "",
                        Note = "",
                        GOT = "true",
                        MnemonicId = (int)MnemonicType.Operation,
                        RecordId = operation.Id,
                        OutcoilNumber = 0
                    };

                    currentMemories.Add(currentMemory);
                    previousMemories.Add(previousMemory);
                    cylinderMemories.Add(cylinderMemory);
                    prosTimesToSave.Add(prosTime);
                }
            }

            // 重複を除去（PlcId, MnemonicId, RecordId, SortIdの組み合わせでユニークにする）
            var uniqueProsTimes = prosTimesToSave
                .GroupBy(pt => new { pt.PlcId, pt.MnemonicId, pt.RecordId, pt.SortId })
                .Select(g => g.First())
                .ToList();

            // 一括で保存
            if (uniqueProsTimes.Any())
            {
                _repository.SaveOrUpdateProsTimesBatch(uniqueProsTimes);
                _repository.SaveOrUpdateMemoriesBatch(currentMemories);
                _repository.SaveOrUpdateMemoriesBatch(previousMemories);
                _repository.SaveOrUpdateMemoriesBatch(cylinderMemories);
            }
        }
    }
}
