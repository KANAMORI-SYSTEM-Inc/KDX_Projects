using Kdx.Contracts.DTOs;
using Kdx.Contracts.Interfaces;

using System.Diagnostics;

namespace KdxDesigner.Services.Memory
{
    /// <summary>
    /// メモリデータの操作を行うサービス実装
    /// </summary>
    internal class MemoryService : IMemoryService
    {
        private readonly IAccessRepository _repository;

        public MemoryService(IAccessRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public List<Kdx.Contracts.DTOs.Memory> GetMemories(int plcId)
        {
            return _repository.GetMemories(plcId);
        }

        public List<MemoryCategory> GetMemoryCategories()
        {
            return _repository.GetMemoryCategories();
        }

        private (int PlcId, string Device) GetMemoryKey(Kdx.Contracts.DTOs.Memory memory)
        {
            return string.IsNullOrEmpty(memory.Device)
                ? throw new ArgumentException("Memory Device cannot be null or empty for key generation.", nameof(memory.Device))
                : (memory.PlcId, memory.Device);
        }

        public void SaveMemories(int plcId, List<Kdx.Contracts.DTOs.Memory> memories, Action<string>? progressCallback = null)
        {
            if (memories == null || !memories.Any())
            {
                progressCallback?.Invoke($"保存対象のメモリデータがありません (PlcId: {plcId})。");
                return;
            }

            try
            {
                // 1. 渡された plcId を使用して、関連する既存レコードを取得
                var existingForThisPlcId = GetMemories(plcId);

                // 2. 取得した既存レコードからルックアップ用辞書を作成
                var existingLookup = new Dictionary<(int PlcId, string Device), Kdx.Contracts.DTOs.Memory>();
                foreach (var mem in existingForThisPlcId)
                {
                    if (mem.PlcId == plcId && !string.IsNullOrEmpty(mem.Device))
                    {
                        existingLookup[GetMemoryKey(mem)] = mem;
                    }
                }

                // 3. 保存対象のメモリデータをフィルタリング
                var memoriesToSave = new List<Kdx.Contracts.DTOs.Memory>();
                int skippedCount = 0;
                
                for (int i = 0; i < memories.Count; i++)
                {
                    var memoryToSave = memories[i];

                    if (memoryToSave == null)
                    {
                        progressCallback?.Invoke($"[{i + 1}/{memories.Count}] スキップ: null のメモリデータです。");
                        skippedCount++;
                        continue;
                    }

                    if (memoryToSave.PlcId != plcId)
                    {
                        progressCallback?.Invoke($"[{i + 1}/{memories.Count}] スキップ: PlcId ({memoryToSave.PlcId.ToString() ?? "null"}) が指定された PlcId ({plcId}) と一致しません。Device: {memoryToSave.Device}");
                        skippedCount++;
                        continue;
                    }

                    if (string.IsNullOrEmpty(memoryToSave.Device))
                    {
                        progressCallback?.Invoke($"[{i + 1}/{memories.Count}] スキップ: Device が null または空です (PlcId: {plcId})。");
                        skippedCount++;
                        continue;
                    }

                    // 有効なメモリデータを保存リストに追加
                    memoriesToSave.Add(memoryToSave);
                }

                // 4. 一括でSupabaseに保存または更新
                if (memoriesToSave.Any())
                {
                    progressCallback?.Invoke($"一括保存中: {memoriesToSave.Count} 件のメモリデータ (PlcId: {plcId})");
                    
                    // バッチ処理で一括保存
                    _repository.SaveOrUpdateMemoriesBatch(memoriesToSave);
                    
                    progressCallback?.Invoke($"メモリデータの保存が完了しました (PlcId: {plcId}, 保存件数: {memoriesToSave.Count}, スキップ: {skippedCount} 件)。");
                }
                else
                {
                    progressCallback?.Invoke($"保存可能なメモリデータがありませんでした (PlcId: {plcId}, スキップ: {skippedCount} 件)。");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] SaveMemories 処理中にエラーが発生しました (PlcId={plcId}): {ex.Message}");
                progressCallback?.Invoke($"エラーが発生しました (PlcId={plcId}): {ex.Message}");
                throw;
            }
        }

        public bool SaveMnemonicMemories(KdxDesigner.Models.MnemonicDevice device)
        {
            if (device?.PlcId == null) return false; // PlcId が必須

            try
            {
                var existingForPlcIdList = GetMemories(device.PlcId);
                var existingLookup = existingForPlcIdList
                    .Where(m => !string.IsNullOrEmpty(m.Device))
                    .ToDictionary(m => m.Device!, m => m); // Deviceで検索 (PlcIdは共通)

                int deviceLabelCategoryId = device.DeviceLabel switch
                {
                    "L" => 1,
                    "M" => 2,
                    "B" => 3,
                    "D" => 4,
                    "ZR" => 5,
                    "W" => 6,
                    "T" => 7,
                    "C" => 8,
                    _ => 1, // TODO: エラー処理または明確なデフォルト値
                };
                string mnemonicTypeBasedCategoryString = device.MnemonicId switch
                {
                    1 => "工程",
                    2 => "工程詳細",
                    3 => "操作",
                    4 => "出力",
                    _ => "なし", // TODO: エラー処理または明確なデフォルト値
                };
                List<Kdx.Contracts.DTOs.Difinitions> difinitions = device.MnemonicId switch
                {
                    1 => _repository.GetDifinitions("Process"),
                    2 => _repository.GetDifinitions("Detail"),
                    3 => _repository.GetDifinitions("Operation"),
                    4 => _repository.GetDifinitions("Cylinder"),
                    _ => new List<Kdx.Contracts.DTOs.Difinitions>(), // TODO: エラー処理または明確なデフォルト値
                };

                for (int i = 0; i < device.OutCoilCount; i++)
                {
                    var deviceNum = device.StartNum + i;
                    var deviceString = device.DeviceLabel + deviceNum.ToString();

                    var memoryToSave = new Kdx.Contracts.DTOs.Memory
                    {
                        PlcId = device.PlcId,
                        MemoryCategory = deviceLabelCategoryId,
                        DeviceNumber = deviceNum,
                        DeviceNumber1 = deviceString,
                        DeviceNumber2 = "",
                        Device = deviceString,
                        Category = mnemonicTypeBasedCategoryString,
                        Row_1 = difinitions.Where(d => d.Label == "").Single(d => d.OutCoilNumber == i).Comment1,
                        Row_2 = difinitions.Single(d => d.OutCoilNumber == i).Comment2,
                        Row_3 = device.Comment2, // Outcoilのインデックスとして
                        Row_4 = device.Comment2,
                        Direct_Input = "",
                        Confirm = mnemonicTypeBasedCategoryString + device.Comment1 + i.ToString(),
                        Note = "",
                        // CreatedAt, UpdatedAt は ExecuteUpsertMemory で処理
                        GOT = "False",
                        MnemonicId = device.MnemonicId, // MnemonicDevice の ID
                        RecordId = device.RecordId, // MnemonicDevice の ID
                        OutcoilNumber = i
                    };

                    existingLookup.TryGetValue(memoryToSave.Device!, out Kdx.Contracts.DTOs.Memory? existingRecord);
                    _repository.SaveOrUpdateMemory(memoryToSave);
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] MnemonicDevice ID={device.ID} のMemory保存失敗 → {ex.Message}");
                return false;
            }
        }

        // SaveMnemonicTimerMemoriesZR と SaveMnemonicTimerMemoriesT も同様のパターンで修正します。
        // Memoryオブジェクトの構築ロジックは各メソッド固有ですが、保存部分はExecuteUpsertMemoryを呼び出します。

        public bool SaveMnemonicTimerMemoriesZR(MnemonicTimerDevice device)
        {
            if (device?.PlcId == null || string.IsNullOrEmpty(device.TimerDeviceZR) || !device.TimerDeviceZR.StartsWith("ZR")) return false;

            var dinitions = new List<Models.Difinitions>();

            try
            {
                var existingForPlcIdList = GetMemories(device.PlcId);
                var existingLookup = existingForPlcIdList.Where(m => !string.IsNullOrEmpty(m.Device))
                                                        .ToDictionary(m => m.Device!, m => m);


                string mnemonicTypeBasedCategoryString = device.MnemonicId switch
                {
                    1 => "工程ﾀｲﾏ",
                    2 => "詳細ﾀｲﾏ",
                    3 => "操作ﾀｲﾏ",
                    4 => "出力ﾀｲﾏ",
                    _ => "なし",
                };

                var tDeviceNumStr = device.TimerDeviceZR.Replace("ZR", "");
                if (int.TryParse(tDeviceNumStr, out int tDeviceNum))
                {
                    var memoryToSave = new Kdx.Contracts.DTOs.Memory
                    {
                        PlcId = device.PlcId,
                        MemoryCategory = 0, // TODO: ZR用の適切なMemoryCategory IDを決定する
                        DeviceNumber = tDeviceNum,
                        DeviceNumber1 = device.TimerDeviceZR,
                        Device = device.TimerDeviceZR,
                        Category = mnemonicTypeBasedCategoryString,
                        Row_1 = mnemonicTypeBasedCategoryString,
                        Row_2 = device.Comment1,
                        Row_3 = device.Comment2,
                        Row_4 = device.Comment3,
                        Note = "",
                        MnemonicId = device.MnemonicId,
                        RecordId = device.RecordId,
                    };

                    existingLookup.TryGetValue(memoryToSave.Device!, out Kdx.Contracts.DTOs.Memory? existingRecord);
                    _repository.SaveOrUpdateMemory(memoryToSave);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] MnemonicTimerDevice MnemonicID={device.MnemonicId} RecordID={device.RecordId} のMemory(ZR)保存失敗 → {ex.Message}");
                return false;
            }
        }

        public bool SaveMnemonicTimerMemoriesT(MnemonicTimerDevice device)
        {

            if (device?.PlcId == null || string.IsNullOrEmpty(device.TimerDeviceT))
            {
                return false;
            }

            try
            {
                var existingForPlcIdList = _repository.GetMemories(device.PlcId);
                var existingLookup = existingForPlcIdList.Where(m => !string.IsNullOrEmpty(m.Device))
                                                        .ToDictionary(m => m.Device!, m => m);

                string mnemonicTypeBasedCategoryString = device.MnemonicId switch
                {
                    1 => "工程タイマT",
                    2 => "工程詳細タイマT",
                    3 => "操作タイマT",
                    4 => "出力タイマT",
                    _ => "タイマT",
                };

                string dDeviceNumStr = string.Empty;

                if (!device.TimerDeviceT.StartsWith("T"))
                {
                    dDeviceNumStr = device.TimerDeviceZR.Replace("T", "");

                }
                else if (!device.TimerDeviceT.StartsWith("ST"))
                {
                    dDeviceNumStr = device.TimerDeviceZR.Replace("ST", "");

                }

                if (int.TryParse(dDeviceNumStr, out int dDeviceNum))
                {
                    var memoryToSave = new Kdx.Contracts.DTOs.Memory
                    {
                        PlcId = device.PlcId,
                        MemoryCategory = 0, // TODO: Tデバイス用の適切なMemoryCategory IDを決定する
                        DeviceNumber = dDeviceNum,
                        DeviceNumber1 = device.TimerDeviceT,
                        Device = device.TimerDeviceT,
                        Category = mnemonicTypeBasedCategoryString,
                        Row_1 = mnemonicTypeBasedCategoryString,
                        Row_2 = device.Comment1,
                        Row_3 = device.Comment2,
                        Row_4 = device.Comment3,
                        MnemonicId = device.MnemonicId,
                        RecordId = device.RecordId,// MnemonicTimerDeviceのIDをMemoryのMnemonicDeviceIdにマッピング
                                                   // 他のフィールドは必要に応じて設定
                    };

                    existingLookup.TryGetValue(memoryToSave.Device!, out Kdx.Contracts.DTOs.Memory? existingRecord);
                    _repository.SaveOrUpdateMemory(memoryToSave);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERROR] MnemonicTimerDevice MnemonicId={device.MnemonicId} RecordId={device.RecordId} のMemory(T)保存失敗 → {ex.Message}");
                return false;
            }
        }

    }
}
