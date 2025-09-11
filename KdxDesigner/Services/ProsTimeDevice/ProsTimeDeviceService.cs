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
            _loadedOperationConfigs = new Dictionary<int, OperationProsTimeConfig>(); // TODO: Supabase対応実装時に初期化
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

            // 一括で保存
            if (prosTimesToSave.Any())
            {
                _repository.SaveOrUpdateProsTimesBatch(prosTimesToSave);
            }
        }
    }
}
