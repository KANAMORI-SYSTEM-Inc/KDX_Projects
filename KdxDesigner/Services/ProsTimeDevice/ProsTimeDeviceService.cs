using Kdx.Contracts.DTOs;
using Kdx.Contracts.Enums;
using Kdx.Contracts.Interfaces;

namespace KdxDesigner.Services.ProsTimeDevice
{
    /// <summary>
    /// ProsTime（工程時間）デバイスの管理サービス実装
    /// </summary>
    internal class ProsTimeDeviceService : IProsTimeDeviceService
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

        // デフォルト設定は引き続き静的メンバーとして保持可能
        private static readonly OperationProsTimeConfig DefaultOperationConfig =
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

        // MnemonicDeviceテーブルからPlcIdとMnemonicIdに基づいてデータを取得する
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
                    // グループ内の TotalCount は全て同じはずなので、最初の要素から取得
                    var totalCount = group.First().TotalCount;

                    var map = new Dictionary<int, int>();
                    foreach (var item in group)
                    {
                        // SortOrder と ResultingCategoryId のマッピングを追加
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
                // データベースアクセスエラー: {ex.Message}
                // エラー発生時はデフォルト設定を返す
                return GetDefaultConfigs();
            }
            
            // 設定が読み込まれたが空の場合もデフォルト設定を使用
            if (!configs.Any())
            {
                // 設定が空のため、デフォルト設定を使用
                return GetDefaultConfigs();
            }
            
            return configs;
        }
        
        /// <summary>
        /// デフォルトのOperation設定を生成
        /// </summary>
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
            
            // {configs.Count}個のデフォルト設定を生成
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

            // 一括保存用のリストを作成
            var prosTimesToSave = new List<ProsTime>();
            int count = 0;

            foreach (Operation operation in operations)
            {
                if (operation == null || operation.CategoryId == null) continue;

                var operationCategoryValue = operation.CategoryId.Value; // Null許容型から値を取得

                // ★変更: _loadedOperationConfigs (インスタンスメンバー) を使用
                OperationProsTimeConfig currentConfig = _loadedOperationConfigs.TryGetValue(operationCategoryValue, out var specificConfig)
                                                        ? specificConfig
                                                        : DefaultOperationConfig;

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
                // 重複除去: 元データ{prosTimesToSave.Count}件 → {uniqueProsTimes.Count}件
                _repository.SaveOrUpdateProsTimesBatch(uniqueProsTimes);
            }
            else
            {
                // 保存するProsTimeがありません
            }
        }
    }
}
