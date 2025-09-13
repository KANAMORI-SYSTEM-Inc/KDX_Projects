using Kdx.Contracts.DTOs;
using Timer = Kdx.Contracts.DTOs.Timer;

namespace Kdx.Infrastructure.Repositories
{
    /// <summary>
    /// Supabaseデータベースへのアクセス機能を提供するリポジトリのインターフェース。
    /// </summary>
    public interface ISupabaseRepository
    {
        /// <summary>
        /// 全ての会社情報を取得します。
        /// </summary>
        Task<List<Company>> GetCompaniesAsync();

        /// <summary>
        /// 全ての機種情報を取得します。
        /// </summary>
        Task<List<Model>> GetModelsAsync();

        /// <summary>
        /// 全てのPLC情報を取得します。
        /// </summary>
        Task<List<PLC>> GetPLCsAsync();

        /// <summary>
        /// 全てのサイクル情報を取得します。
        /// </summary>
        Task<List<Cycle>> GetCyclesAsync();

        /// <summary>
        /// CylinderとCycleの中間テーブルをplcIdを元に取得します。
        /// </summary>
        /// <param name="plcId"></param>
        /// <returns></returns>
        Task<List<CylinderCycle>> GetCylinderCyclesByPlcIdAsync(int plcId);

        /// <summary>
        /// 全ての工程情報を取得します。
        /// </summary>
        Task<List<Process>> GetProcessesAsync();
        
        /// <summary>
        /// 新しい工程情報を追加します。
        /// </summary>
        /// <param name="process">追加するProcessオブジェクト。</param>
        /// <returns>追加されたレコードのID。</returns>
        Task<int> AddProcessAsync(Process process);
        
        /// <summary>
        /// 工程情報を更新します。
        /// </summary>
        /// <param name="process">更新するProcessオブジェクト。</param>
        Task UpdateProcessAsync(Process process);

        /// <summary>
        /// 全ての機械情報を取得します。
        /// </summary>
        Task<List<Machine>> GetMachinesAsync();

        /// <summary>
        /// idで指定された機械情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Machine?> GetMachineByIdAsync(int nameId, int driveSubId);

        /// <summary>
        /// idで指定された機械名称情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns>MachineName?</returns>
        Task<MachineName?> GetMachineNameByIdAsync(int id);

        /// <summary>
        /// 全ての駆動部(主)情報を取得します。
        /// </summary>
        Task<List<DriveMain>> GetDriveMainsAsync();

        /// <summary>
        /// 全ての駆動部(副)情報を取得します。
        /// </summary>
        Task<List<DriveSub>> GetDriveSubsAsync();

        /// <summary>
        /// 駆動部(副)情報をidで指定して取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DriveSub?> GetDriveSubByIdAsync(int id);

        /// <summary>
        /// 全てのシリンダー(CY)情報を取得します。
        /// </summary>
        Task<List<Cylinder>> GetCYsAsync();
        
        /// <summary>
        /// 指定されたPLC IDに紐づくシリンダー(CY)情報を取得します。
        /// </summary>
        /// <param name="plcId">取得対象のPLC ID。</param>
        Task<List<Cylinder>> GetCyListAsync(int plcId);

        /// <summary>
        /// idで指定されたシリンダー(CY)情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Cylinder?> GetCYByIdAsync(int id);

        /// <summary>
        /// 指定されたサイクルIDに紐づくタイマー情報を取得します。
        /// </summary
        Task<List<Timer>> GetTimersAsync();

        /// <summary>
        /// 指定されたサイクルIDに紐づくタイマー情報を取得します。
        /// </summary>
        /// <param name="cycleId">取得対象のサイクルID。</param>
        Task<List<Timer>> GetTimersByCycleIdAsync(int cycleId);

        /// <summary>
        /// 指定されたサイクルIDに紐づくタイマー情報を取得します。
        /// </summary>
        /// <param name="cycleId">取得対象のサイクルID。</param>
        Task<List<MnemonicTimerDevice>> GetTimersByRecordIdAsync(int cycleId, int mnemonicId, int recordId);

        /// <summary>
        /// 新しいタイマーを追加します。
        /// </summary>
        /// <param name="timer">追加するタイマー情報。</param>
        Task AddTimerAsync(Timer timer);

        /// <summary>
        /// タイマー情報を更新します。
        /// </summary>
        /// <param name="timer">更新するタイマー情報。</param>
        Task UpdateTimerAsync(Timer timer);

        /// <summary>
        /// 指定されたIDのタイマーを削除します。
        /// </summary>
        /// <param name="id">削除するタイマーのID。</param>
        Task DeleteTimerAsync(int id);

        /// <summary>
        /// 指定されたタイマーIDに関連するレコードIDを取得します。
        /// </summary>
        /// <param name="timerId">タイマーID。</param>
        Task<List<int>> GetTimerRecordIdsAsync(int timerId);

        /// <summary>
        /// タイマーとレコードの関連を追加します。
        /// </summary>
        /// <param name="timerId">タイマーID。</param>
        /// <param name="recordId">レコードID。</param>
        Task AddTimerRecordIdAsync(int timerId, int recordId);

        /// <summary>
        /// タイマーとレコードの関連を削除します。
        /// </summary>
        /// <param name="timerId">タイマーID。</param>
        /// <param name="recordId">レコードID。</param>
        Task DeleteTimerRecordIdAsync(int timerId, int recordId);

        /// <summary>
        /// 指定されたタイマーIDの全レコード関連を削除します。
        /// </summary>
        /// <param name="timerId">タイマーID。</param>
        Task DeleteAllTimerRecordIdsAsync(int timerId);

        /// <summary>
        /// 全ての操作情報を取得します。
        /// </summary>
        Task<List<Operation>> GetOperationsAsync();

        /// <summary>
        /// 指定されたサイクルIDの操作情報を取得します。
        /// </summary>
        /// <param name="cycleId">サイクルID</param>
        /// <returns>該当する操作のリスト</returns>
        Task<List<Operation>> GetOperationsByCycleIdAsync(int cycleId);

        /// <summary>
        /// 指定されたIDの操作情報を取得します。
        /// </summary>
        /// <param name="id">取得対象の操作ID。</param>
        /// <returns>見つかった場合はOperationオブジェクト、見つからない場合はnull。</returns>
        Task<Operation?> GetOperationByIdAsync(int id);

        /// <summary>
        /// 指定されたPLC IDに紐づくLength情報を取得します。
        /// </summary>
        /// <param name="plcId">取得対象のPLC ID。</param>
        Task<List<Length>?> GetLengthByPlcIdAsync(int plcId);

        /// <summary>
        /// 新しい操作情報を追加します。
        /// </summary>
        /// <param name="operation">追加するOperationオブジェクト。</param>
        /// <returns>追加されたレコードのID。</returns>
        Task<int> AddOperationAsync(Operation operation);

        /// <summary>
        /// 指定された操作情報を更新します。
        /// </summary>
        /// <param name="operation">更新するOperationオブジェクト。</param>
        Task UpdateOperationAsync(Operation operation);

        /// <summary>
        /// 全ての工程詳細情報を取得します。
        /// </summary>
        Task<List<ProcessDetail>> GetProcessDetailsAsync();

        /// <summary>
        /// 指定された工程詳細情報を更新します。
        /// </summary>
        /// <param name="processDetail">更新するProcessDetailオブジェクト。</param>
        Task UpdateProcessDetailAsync(ProcessDetail processDetail);

        /// <summary>
        /// 新しい工程詳細情報を追加します。
        /// </summary>
        /// <param name="processDetail">追加するProcessDetailオブジェクト。</param>
        /// <returns>追加されたレコードのID。</returns>
        Task<int> AddProcessDetailAsync(ProcessDetail processDetail);

        /// <summary>
        /// 指定されたIDの工程情報を削除します。
        /// </summary>
        /// <param name="id">削除対象の工程ID。</param>
        Task DeleteProcessAsync(int id);

        /// <summary>
        /// 指定されたIDの工程詳細情報を削除します。
        /// </summary>
        /// <param name="id">削除対象の工程詳細ID。</param>
        Task DeleteProcessDetailAsync(int id);

        /// <summary>
        /// 指定されたIDの操作情報を削除します。
        /// </summary>
        /// <param name="id">削除対象の操作ID。</param>
        Task DeleteOperationAsync(int id);

        /// <summary>
        /// 全ての工程カテゴリを取得します。
        /// </summary>
        Task<List<ProcessCategory>> GetProcessCategoriesAsync();
        
        /// <summary>
        /// 全ての工程詳細カテゴリを取得します。
        /// </summary>
        Task<List<ProcessDetailCategory>> GetProcessDetailCategoriesAsync();

        /// <summary>
        /// idで指定されたの工程詳細カテゴリを取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcessDetailCategory?> GetProcessDetailCategoryByIdAsync(int id);
        
        /// <summary>
        /// 全ての操作カテゴリを取得します。
        /// </summary>
        Task<List<OperationCategory>> GetOperationCategoriesAsync();

        /// <summary>
        /// 全てのMnemonicTimerDevice情報を取得します。
        /// </summary>
        Task<List<MnemonicTimerDevice>> GetMnemonicTimerDevicesAsync();

        /// <summary>
        /// 全てのMnemonicTimerDevice情報を取得します。
        /// </summary>
        Task<List<MnemonicTimerDevice>> GetMnemonicTimerDevicesByClcleIdAsync(int plcId, int cycleId);

        /// <summary>
        /// 全てのMnemonicTimerDevice情報を取得します。
        /// </summary>
        Task<List<MnemonicTimerDevice>> GetMnemonicTimerDevicesByCycleAndMnemonicIdAsync(int plcId, int cycleId, int mnemonicId);

        Task<List<MnemonicTimerDevice>> GetMnemonicTimerDevicesByMnemonicIdAsync(int plcId, int mnemonicId);

        Task<List<MnemonicTimerDevice>> GetMnemonicTimerDevicesByTimerIdAsync(int plcId, int timerId);

        /// <summary>
        /// 指定されたMnemonicTimerDeviceを更新します。
        /// </summary>
        Task UpdateMnemonicTimerDeviceAsync(MnemonicTimerDevice device);

        Task DeleteAllMnemonicTimerDeviceAsync();

        /// <summary>
        /// 指定されたMnemonicTimerDeviceを削除します。
        /// </summary>
        Task DeleteMnemonicTimerDeviceAsync(int mnemonicId, int recordId, int timerId);

        /// <summary>
        /// 新しいMnemonicTimerDeviceを追加します。
        /// </summary>
        Task AddMnemonicTimerDeviceAsync(MnemonicTimerDevice device);

        /// <summary>
        /// MnemonicTimerDeviceを挿入または更新します。
        /// </summary>
        Task UpsertMnemonicTimerDeviceAsync(MnemonicTimerDevice device);

        /// <summary>
        /// 全てのIOリスト情報を取得します。
        /// </summary>
        Task<List<IO>> GetIoListAsync();

        /// <summary>
        /// 全てのタイマーカテゴリ情報を取得します。
        /// </summary>
        Task<List<TimerCategory>> GetTimerCategoryAsync();

        /// <summary>
        /// サーボ情報を取得します。
        /// </summary>
        Task<List<Servo>> GetServosAsync(int? plcId, int? cylinderId);

        /// <summary>
        /// IOレコードのリストを受け取り、LinkDeviceカラムをバッチ更新します。
        /// </summary>
        /// <param name="ioRecordsToUpdate">LinkDeviceが設定されたIOオブジェクトのリスト。</param>
        Task UpdateIoLinkDevicesAsync(IEnumerable<IO> ioRecordsToUpdate);

        /// <summary>
        /// IOレコードのリストを更新し、同時に変更履歴を保存します。
        /// これらの一連の処理は単一のトランザクション内で実行されます。
        /// </summary>
        /// <param name="iosToUpdate">更新対象のIOオブジェクトのリスト。</param>
        /// <param name="histories">保存する変更履歴のリスト。</param>
        Task UpdateAndLogIoChangesAsync(List<IO> iosToUpdate, List<IOHistory> histories);

        /// <summary>
        /// 指定されたサイクルIDに紐づく工程詳細接続情報を取得します。
        /// </summary>
        /// <param name="cycleId">取得対象のサイクルID。</param>
        Task<List<ProcessDetailConnection>> GetProcessDetailConnectionsAsync(int cycleId);
        Task<List<ProcessDetailConnection>> GetAllProcessDetailConnectionsAsync();

        /// <summary>
        /// 指定されたToProcessDetailIdに紐づく接続情報を取得します。
        /// </summary>
        /// <param name="toProcessDetailId">終点の工程詳細ID。</param>
        Task<List<ProcessDetailConnection>> GetConnectionsByToIdAsync(int toProcessDetailId);

        /// <summary>
        /// 指定されたFromProcessDetailIdに紐づく接続情報を取得します。
        /// </summary>
        /// <param name="fromProcessDetailId">始点の工程詳細ID。</param>
        Task<List<ProcessDetailConnection>> GetConnectionsByFromIdAsync(int fromProcessDetailId);

        /// <summary>
        /// 新しい工程詳細接続情報を追加します。
        /// </summary>
        /// <param name="connection">追加するProcessDetailConnectionオブジェクト。</param>
        Task AddProcessDetailConnectionAsync(ProcessDetailConnection connection);

        /// <summary>
        /// 指定されたIDの工程詳細接続情報を削除します。
        /// </summary>
        /// <param name="id">削除対象の接続ID。</param>
        Task DeleteProcessDetailConnectionAsync(int id);

        /// <summary>
        /// 指定されたFromIdとToIdの組み合わせの接続情報を削除します。
        /// </summary>
        /// <param name="fromId">始点の工程詳細ID。</param>
        /// <param name="toId">終点の工程詳細ID。</param>
        Task DeleteConnectionsByFromAndToAsync(int fromId, int toId);

        /// <summary>
        /// 指定されたサイクルIDに紐づく工程詳細終了情報を取得します。
        /// </summary>
        /// <param name="cycleId">取得対象のサイクルID。</param>
        Task<List<ProcessDetailFinish>> GetProcessDetailFinishesAsync(int cycleId);
        Task<List<ProcessDetailFinish>> GetAllProcessDetailFinishesAsync();

        /// <summary>
        /// 指定されたProcessDetailIdに紐づく終了情報を取得します。
        /// </summary>
        /// <param name="processDetailId">工程詳細ID。</param>
        Task<List<ProcessDetailFinish>> GetFinishesByProcessDetailIdAsync(int processDetailId);

        /// <summary>
        /// 指定されたFinishProcessDetailIdに紐づく終了情報を取得します。
        /// </summary>
        /// <param name="finishProcessDetailId">終了先の工程詳細ID。</param>
        Task<List<ProcessDetailFinish>> GetFinishesByFinishIdAsync(int finishProcessDetailId);

        /// <summary>
        /// 新しい工程詳細終了情報を追加します。
        /// </summary>
        /// <param name="finish">追加するProcessDetailFinishオブジェクト。</param>
        Task AddProcessDetailFinishAsync(ProcessDetailFinish finish);

        /// <summary>
        /// 指定されたIDの工程詳細終了情報を削除します。
        /// </summary>
        /// <param name="id">削除対象の終了情報ID。</param>
        Task DeleteProcessDetailFinishAsync(int id);

        /// <summary>
        /// 指定されたProcessDetailIdとFinishProcessDetailIdの組み合わせの終了情報を削除します。
        /// </summary>
        /// <param name="processDetailId">工程詳細ID。</param>
        /// <param name="finishProcessDetailId">終了先の工程詳細ID。</param>
        Task DeleteFinishesByProcessAndFinishAsync(int processDetailId, int finishProcessDetailId);

        #region ProcessStartCondition Methods

        /// <summary>
        /// 指定されたサイクルIDの全ての工程開始条件を取得します。
        /// </summary>
        /// <param name="cycleId">サイクルID。</param>
        Task<List<ProcessStartCondition>> GetProcessStartConditionsAsync(int cycleId);

        /// <summary>
        /// 指定された工程IDの開始条件を取得します。
        /// </summary>
        /// <param name="processId">工程ID。</param>
        Task<List<ProcessStartCondition>> GetStartConditionsByProcessIdAsync(int processId);

        /// <summary>
        /// 新しい工程開始条件を追加します。
        /// </summary>
        /// <param name="condition">追加する開始条件。</param>
        Task AddProcessStartConditionAsync(ProcessStartCondition condition);

        /// <summary>
        /// 指定されたIDの工程開始条件を削除します。
        /// </summary>
        /// <param name="id">削除対象のID。</param>
        Task DeleteProcessStartConditionAsync(int id);

        /// <summary>
        /// 指定された工程IDの全ての開始条件を削除します。
        /// </summary>
        /// <param name="processId">工程ID。</param>
        Task DeleteStartConditionsByProcessAsync(int processId);

        #endregion

        #region ProcessFinishCondition Methods

        /// <summary>
        /// 指定されたサイクルIDの全ての工程終了条件を取得します。
        /// </summary>
        /// <param name="cycleId">サイクルID。</param>
        Task<List<ProcessFinishCondition>> GetProcessFinishConditionsAsync(int cycleId);

        /// <summary>
        /// 指定された工程IDの終了条件を取得します。
        /// </summary>
        /// <param name="processId">工程ID。</param>
        Task<List<ProcessFinishCondition>> GetFinishConditionsByProcessIdAsync(int processId);

        /// <summary>
        /// 新しい工程終了条件を追加します。
        /// </summary>
        /// <param name="condition">追加する終了条件。</param>
        Task AddProcessFinishConditionAsync(ProcessFinishCondition condition);

        /// <summary>
        /// 指定されたIDの工程終了条件を削除します。
        /// </summary>
        /// <param name="id">削除対象のID。</param>
        Task DeleteProcessFinishConditionAsync(int id);

        /// <summary>
        /// 指定された工程IDの全ての終了条件を削除します。
        /// </summary>
        /// <param name="processId">工程ID。</param>
        Task DeleteFinishConditionsByProcessAsync(int processId);

        #endregion

        #region CylinderIO Methods
        
        Task<List<CylinderIO>> GetCylinderIOsAsync(int cylinderId, int plcId);
        Task<List<CylinderIO>> GetIOCylindersAsync(string ioAddress, int plcId);
        Task AddCylinderIOAssociationAsync(int cylinderId, string ioAddress, int plcId, string ioType, string? comment = null);
        Task RemoveCylinderIOAssociationAsync(int cylinderId, string ioAddress, int plcId);
        Task<List<CylinderIO>> GetAllCylinderIOAssociationsAsync(int plcId);
        
        #endregion

        #region OperationIO Methods
        
        Task<List<OperationIO>> GetOperationIOsAsync(int operationId);
        Task<List<OperationIO>> GetIOOperationsAsync(string ioAddress, int plcId);
        Task AddOperationIOAssociationAsync(int operationId, string ioAddress, int plcId, string ioUsage, string? comment = null);
        Task RemoveOperationIOAssociationAsync(int operationId, string ioAddress, int plcId);
        Task<List<OperationIO>> GetAllOperationIOAssociationsAsync(int plcId);
        
        #endregion

        #region Error Methods
        
        Task DeleteErrorTableAsync();
        Task<List<ErrorMessage>> GetErrorMessagesAsync(int mnemonicId);
        Task<List<ProcessError>> GetErrorsAsync(int plcId, int cycleId, int mnemonicId);

        Task SaveErrorsAsync(List<ProcessError> errors);

        Task UpdateErrorsAsync(List<ProcessError> errors);

        
        #endregion

        #region MnemonicDevice Methods
        
        Task<List<MnemonicDevice>> GetMnemonicDevicesAsync(int plcId);
        Task<List<MnemonicDevice>> GetMnemonicDevicesByMnemonicAsync(int plcId, int mnemonicId);
        Task DeleteMnemonicDeviceAsync(int plcId, int mnemonicId);
        Task DeleteAllMnemonicDevicesAsync();
        Task SaveOrUpdateMnemonicDeviceAsync(MnemonicDevice device);
        
        #endregion

        #region MnemonicSpeedDevice Methods
        
        Task<List<MnemonicSpeedDevice>> GetMnemonicSpeedDevicesAsync(int plcId);
        Task DeleteSpeedTableAsync();
        Task SaveOrUpdateMnemonicSpeedDeviceAsync(MnemonicSpeedDevice device);
        
        #endregion

        #region ProsTime Methods
        
        Task<List<ProsTime>> GetProsTimeByPlcIdAsync(int plcId);
        Task<List<ProsTime>> GetProsTimeByMnemonicIdAsync(int plcId, int mnemonicId);
        Task DeleteProsTimeTableAsync();
        Task SaveOrUpdateProsTimeAsync(ProsTime prosTime);
        Task SaveOrUpdateProsTimesBatchAsync(List<ProsTime> prosTimes);
        Task<List<ProsTimeDefinitions>> GetProsTimeDefinitionsAsync();
        
        #endregion

        #region Memory Methods
        
        Task<List<Memory>> GetMemoriesAsync(int plcId);
        Task<List<MemoryCategory>> GetMemoryCategoriesAsync();
        Task SaveOrUpdateMemoryAsync(Memory memory);
        Task SaveOrUpdateMemoriesBatchAsync(List<Memory> memories);
        
        #endregion

        #region Difinitions Methods
        
        Task<List<Difinitions>> GetDifinitionsAsync(string category);
        Task<Difinitions?> GetDifinitionAsync(string category);


        #endregion
    }
}
