using Kdx.Contracts.DTOs;
using Kdx.Contracts.Enums;
using Kdx.Contracts.Interfaces;

namespace KdxDesigner.Services.ErrorService
{
    /// <summary>
    /// エラー情報のデータ操作を行うサービス実装
    /// </summary>
    internal class ErrorService : IErrorService
    {
        private readonly IAccessRepository _repository;

        public ErrorService(IAccessRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void DeleteErrorTable()
        {
            _repository.DeleteErrorTable();
        }

        public List<Kdx.Contracts.DTOs.ProcessError> GetErrors(int plcId, int cycleId, int mnemonicId)
        {
            return _repository.GetErrors(plcId, cycleId, mnemonicId);
        }

        // Operationのリストを受け取り、Errorテーブルに保存する
        public void SaveMnemonicDeviceOperation(
            List<Kdx.Contracts.DTOs.Operation> operations,
            List<Kdx.Contracts.DTOs.IO> iOs,
            int startNum,
            int startNumTimer,
            int plcId,
            int cycleId
            )
        {
            // MnemonicDeviceテーブルの既存データを取得
            var allExisting = GetErrors(plcId, cycleId, (int)MnemonicType.Operation);
            var messages = _repository.GetErrorMessages((int)MnemonicType.Operation);

            int alarmCount = 0;
            foreach (Operation operation in operations)
            {
                if (operation == null) continue;
                var existing = allExisting.FirstOrDefault(m => m.RecordId == operation.Id);
                var category = operation.CategoryId;

                List<int> AlarmIds = new();
                switch (category)
                {
                    case 2 or 29 or 30: // 保持
                        AlarmIds.AddRange([1, 2, 5]);
                        break;
                    case 3 or 9 or 15 or 27: // 速度制御INV1
                        AlarmIds.AddRange([1, 2, 3, 4, 5]);
                        break;
                    case 4 or 10 or 16 or 28: // 速度制御INV2
                        AlarmIds.AddRange([1, 2, 3, 4, 3, 4, 5]);
                        break;
                    case 5 or 11 or 17:     // 速度制御INV3
                        AlarmIds.AddRange([1, 2, 3, 4, 3, 4, 3, 4, 5]);
                        break;
                    case 6 or 12 or 18: // 速度制御INV4
                        AlarmIds.AddRange([1, 2, 3, 4, 3, 4, 3, 4, 3, 4, 5]);
                        break;
                    case 7 or 13 or 19: // 速度制御INV5
                        AlarmIds.AddRange([1, 2, 3, 4, 3, 4, 3, 4, 3, 4, 3, 4, 5]);
                        break;
                    case 20:            // バネ
                        AlarmIds.AddRange([5]);
                        break;
                    case 31:            // サーボ
                        break;
                    default:
                        break;
                }

                List<Kdx.Contracts.DTOs.ProcessError> insertErrors = new();
                List<Kdx.Contracts.DTOs.ProcessError> updateErrors = new();

                foreach (int id in AlarmIds)
                {
                    string device = "M" + (startNum + alarmCount).ToString(); // 例: 01A01, 01A02, ...
                    string timerDevice = "T" + (startNumTimer + alarmCount).ToString(); // 例: T01A01, T01A02, ...


                    string comment = messages.FirstOrDefault(m => m.AlarmId == id)?.BaseMessage ?? string.Empty;
                    string alarm = messages.FirstOrDefault(m => m.AlarmId == id)?.BaseAlarm ?? string.Empty;
                    int count = messages.FirstOrDefault(m => m.AlarmId == id)?.DefaultCountTime ?? 1000;

                    var comment2 = operation.Valve1 + operation.GoBack;
                    var comment3 = messages.FirstOrDefault(m => m.AlarmId == id)?.Category2 ?? string.Empty;
                    var comment4 = messages.FirstOrDefault(m => m.AlarmId == id)?.Category3 ?? string.Empty;

                    Kdx.Contracts.DTOs.ProcessError saveError = new()
                    {
                        PlcId = plcId,
                        CycleId = cycleId,
                        Device = device,
                        MnemonicId = (int)MnemonicType.Operation,
                        RecordId = operation.Id,
                        AlarmId = id,
                        ErrorNum = alarmCount,
                        Comment1 = "操作ｴﾗｰ",
                        Comment2 = comment2,
                        Comment3 = comment3,
                        Comment4 = comment4,
                        AlarmComment = alarm,
                        MessageComment = comment,
                        ErrorTime = count,
                        ErrorTimeDevice = timerDevice
                    };


                    if (existing != null)
                    {
                        // 既存のレコードがある場合はIDを引き継ぐ
                        updateErrors.Add(saveError);
                    }
                    else
                    {
                        insertErrors.Add(saveError);
                    }

                    // 将来的にメッセージの代入処理を追加する。

                    alarmCount++;
                }

                _repository.SaveErrors(insertErrors);
                _repository.UpdateErrors(updateErrors);
            }
        }
    }
}
