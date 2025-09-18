using Kdx.Contracts.DTOs;
using Timer = Kdx.Contracts.DTOs.Timer;

namespace Kdx.Contracts.Interfaces
{
    /// <summary>
    /// データベースへのアクセス機能を提供するリポジトリのインターフェース。
    /// </summary>
    public interface IAccessRepository
    {
        /// <summary>
        /// データベースへの接続文字列を取得します。
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// 全ての会社情報を取得します。
        /// </summary>
        List<Company> GetCompanies();

        /// <summary>
        /// 全ての機種情報を取得します。
        /// </summary>
        List<Model> GetModels();

        /// <summary>
        /// 全てのPLC情報を取得します。
        /// </summary>
        List<PLC> GetPLCs();

        /// <summary>
        /// 全てのサイクル情報を取得します。
        /// </summary>
        List<Cycle> GetCycles();

        /// <summary>
        /// 全てのサイクル情報を取得します。
        /// </summary>
        List<CylinderCycle>? GetCylinderCyclesByPlcId(int plcId);

        List<ControlBox> GetControlBoxesByPlcId(int plcId);

        List<CylinderControlBox> GetCylinderControlBoxesByPlcId(int plcId);

        List<CylinderControlBox> GetCylinderControlBoxes(int plcId, int cylinderId);

        void UpsertCylinderControlBox(CylinderControlBox list);

        void DeleteCylinderControlBox(int plcId, int cylinderId, int boxId);


        /// <summary>
        /// 全ての工程情報を取得します。
        /// </summary>
        List<Process> GetProcesses();
        
        /// <summary>
        /// 新しい工程情報を追加します。
        /// </summary>
        /// <param name="process">追加するProcessオブジェクト。</param>
        /// <returns>追加されたレコードのID。</returns>
        int AddProcess(Process process);
        
        /// <summary>
        /// 工程情報を更新します。
        /// </summary>
        /// <param name="process">更新するProcessオブジェクト。</param>
        void UpdateProcess(Process process);

        /// <summary>
        /// 全ての機械情報を取得します。
        /// </summary>
        List<Machine> GetMachines();

        List<MachineName> GetMachineNames();

        /// <summary>
        /// idで指定された機械情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MachineName? GetMachineNameById(int id);

        Machine? GetMachineById(int nameId, int driveSubId);


        /// <summary>
        /// 全ての駆動部(主)情報を取得します。
        /// </summary>
        List<DriveMain> GetDriveMains();

        /// <summary>
        /// 全ての駆動部(副)情報を取得します。
        /// </summary>
        List<DriveSub> GetDriveSubs();

        /// <summary>
        /// 駆動部(副)情報をidで指定して取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DriveSub? GetDriveSubById(int id);

        /// <summary>
        /// 全てのシリンダー(CY)情報を取得します。
        /// </summary>
        List<Cylinder> GetCYs();
        
        /// <summary>
        /// 指定されたPLC IDに紐づくシリンダー(CY)情報を取得します。
        /// </summary>
        /// <param name="plcId">取得対象のPLC ID。</param>
        List<Cylinder> GetCyList(int plcId);

        /// <summary>
        /// idで指定されたシリンダー(CY)情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Cylinder? GetCYById(int id);

        /// <summary>
        /// 指定されたサイクルIDに紐づくタイマー情報を取得します。
        /// </summary
        List<Kdx.Contracts.DTOs.Timer> GetTimers();

        /// <summary>
        /// 指定されたサイクルIDに紐づくタイマー情報を取得します。
        /// </summary>
        /// <param name="cycleId">取得対象のサイクルID。</param>
        List<Timer> GetTimersByCycleId(int cycleId);

        /// <summary>
        /// 指定されたサイクルIDに紐づくタイマー情報を取得します。
        /// </summary>
        /// <param name="cycleId">取得対象のサイクルID。</param>
        List<MnemonicTimerDevice> GetTimersByRecordId(int cycleId, int mnemonicId, int recordId);

        /// <summary>
        /// 新しいタイマーを追加します。
        /// </summary>
        /// <param name="timer">追加するタイマー情報。</param>
        void AddTimer(Timer timer);

        /// <summary>
        /// タイマー情報を更新します。
        /// </summary>
        /// <param name="timer">更新するタイマー情報。</param>
        void UpdateTimer(Timer timer);

        /// <summary>
        /// 指定されたIDのタイマーを削除します。
        /// </summary>
        /// <param name="id">削除するタイマーのID。</param>
        void DeleteTimer(int id);

        /// <summary>
        /// 指定されたタイマーIDに関連するレコードIDを取得します。
        /// </summary>
        /// <param name="timerId">タイマーID。</param>
        List<int> GetTimerRecordIds(int timerId);

        /// <summary>
        /// タイマーとレコードの関連を追加します。
        /// </summary>
        /// <param name="timerId">タイマーID。</param>
        /// <param name="recordId">レコードID。</param>
        void AddTimerRecordId(int timerId, int recordId);

        /// <summary>
        /// タイマーとレコードの関連を削除します。
        /// </summary>
        /// <param name="timerId">タイマーID。</param>
        /// <param name="recordId">レコードID。</param>
        void DeleteTimerRecordId(int timerId, int recordId);

        /// <summary>
        /// 指定されたタイマーIDの全レコード関連を削除します。
        /// </summary>
        /// <param name="timerId">タイマーID。</param>
        void DeleteAllTimerRecordIds(int timerId);

        /// <summary>
        /// 全ての操作情報を取得します。
        /// </summary>
        List<Operation> GetOperations();

        /// <summary>
        /// 指定されたサイクルIDの操作情報を取得します。
        /// </summary>
        /// <param name="cycleId">サイクルID</param>
        /// <returns>該当する操作のリスト</returns>
        List<Operation> GetOperationsByCycleId(int cycleId);

        /// <summary>
        /// 指定されたIDの操作情報を取得します。
        /// </summary>
        /// <param name="id">取得対象の操作ID。</param>
        /// <returns>見つかった場合はOperationオブジェクト、見つからない場合はnull。</returns>
        Operation? GetOperationById(int id);

        /// <summary>
        /// 指定されたPLC IDに紐づくLength情報を取得します。
        /// </summary>
        /// <param name="plcId">取得対象のPLC ID。</param>
        List<Length>? GetLengthByPlcId(int plcId);

        /// <summary>
        /// 新しい操作情報を追加します。
        /// </summary>
        /// <param name="operation">追加するOperationオブジェクト。</param>
        /// <returns>追加されたレコードのID。</returns>
        int AddOperation(Operation operation);

        /// <summary>
        /// 指定された操作情報を更新します。
        /// </summary>
        /// <param name="operation">更新するOperationオブジェクト。</param>
        void UpdateOperation(Operation operation);

        /// <summary>
        /// 全ての工程詳細情報を取得します。
        /// </summary>
        List<ProcessDetail> GetProcessDetails();

        /// <summary>
        /// 指定された工程詳細情報を更新します。
        /// </summary>
        /// <param name="processDetail">更新するProcessDetailオブジェクト。</param>
        void UpdateProcessDetail(ProcessDetail processDetail);

        /// <summary>
        /// 新しい工程詳細情報を追加します。
        /// </summary>
        /// <param name="processDetail">追加するProcessDetailオブジェクト。</param>
        /// <returns>追加されたレコードのID。</returns>
        int AddProcessDetail(ProcessDetail processDetail);

        /// <summary>
        /// 指定されたIDの工程情報を削除します。
        /// </summary>
        /// <param name="id">削除対象の工程ID。</param>
        void DeleteProcess(int id);

        /// <summary>
        /// 指定されたIDの工程詳細情報を削除します。
        /// </summary>
        /// <param name="id">削除対象の工程詳細ID。</param>
        void DeleteProcessDetail(int id);

        /// <summary>
        /// 指定されたIDの操作情報を削除します。
        /// </summary>
        /// <param name="id">削除対象の操作ID。</param>
        void DeleteOperation(int id);

        /// <summary>
        /// 全ての工程カテゴリを取得します。
        /// </summary>
        List<ProcessCategory> GetProcessCategories();
        
        /// <summary>
        /// 全ての工程詳細カテゴリを取得します。
        /// </summary>
        List<ProcessDetailCategory> GetProcessDetailCategories();

        /// <summary>
        /// idで指定されたの工程詳細カテゴリを取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProcessDetailCategory? GetProcessDetailCategoryById(int id);
        
        /// <summary>
        /// 全ての操作カテゴリを取得します。
        /// </summary>
        List<OperationCategory> GetOperationCategories();

        /// <summary>
        /// 全てのMnemonicTimerDevice情報を取得します。
        /// </summary>
        List<MnemonicTimerDevice> GetMnemonicTimerDevices();

        List<MnemonicTimerDevice> GetMnemonicTimerDevicesByCycleId(int plcId, int cycleId);

        List<MnemonicTimerDevice> GetMnemonicTimerDevicesByCycleAndMnemonicId(int plcId, int cycleId, int mnemonicId);

        List<MnemonicTimerDevice> GetMnemonicTimerDevicesByMnemonicId(int plcId, int mnemonicId);

        List<MnemonicTimerDevice> GetMnemonicTimerDevicesByTimerId(int plcId, int timerId);

        void DeleteAllMnemonicTimerDevices();

        /// <summary>
        /// 指定されたMnemonicTimerDeviceを更新します。
        /// </summary>
        void UpdateMnemonicTimerDevice(MnemonicTimerDevice device);

        /// <summary>
        /// 指定されたMnemonicTimerDeviceを削除します。
        /// </summary>
        void DeleteMnemonicTimerDevice(int mnemonicId, int recordId, int timerId);

        /// <summary>
        /// 新しいMnemonicTimerDeviceを追加します。
        /// </summary>
        void AddMnemonicTimerDevice(MnemonicTimerDevice device);

        /// <summary>
        /// MnemonicTimerDeviceを挿入または更新します。
        /// </summary>
        void UpsertMnemonicTimerDevice(MnemonicTimerDevice device);

        /// <summary>
        /// 全てのIOリスト情報を取得します。
        /// </summary>
        List<IO> GetIoList();

        /// <summary>
        /// 全てのタイマーカテゴリ情報を取得します。
        /// </summary>
        List<TimerCategory> GetTimerCategory();

        /// <summary>
        /// サーボ情報を取得します。
        /// </summary>
        List<Servo> GetServos(int? plcId, int? cylinderId);

        /// <summary>
        /// IOレコードのリストを受け取り、LinkDeviceカラムをバッチ更新します。
        /// </summary>
        /// <param name="ioRecordsToUpdate">LinkDeviceが設定されたIOオブジェクトのリスト。</param>
        void UpdateIoLinkDevices(IEnumerable<IO> ioRecordsToUpdate);

        /// <summary>
        /// IOレコードのリストを更新し、同時に変更履歴を保存します。
        /// これらの一連の処理は単一のトランザクション内で実行されます。
        /// </summary>
        /// <param name="iosToUpdate">更新対象のIOオブジェクトのリスト。</param>
        /// <param name="histories">保存する変更履歴のリスト。</param>
        void UpdateAndLogIoChanges(List<IO> iosToUpdate, List<IOHistory> histories);

        /// <summary>
        /// 指定されたサイクルIDに紐づく工程詳細接続情報を取得します。
        /// </summary>
        /// <param name="cycleId">取得対象のサイクルID。</param>
        List<ProcessDetailConnection> GetProcessDetailConnections(int cycleId);
        List<ProcessDetailConnection> GetAllProcessDetailConnections();

        /// <summary>
        /// 指定されたToProcessDetailIdに紐づく接続情報を取得します。
        /// </summary>
        /// <param name="toProcessDetailId">終点の工程詳細ID。</param>
        List<ProcessDetailConnection> GetConnectionsByToId(int toProcessDetailId);

        /// <summary>
        /// 指定されたFromProcessDetailIdに紐づく接続情報を取得します。
        /// </summary>
        /// <param name="fromProcessDetailId">始点の工程詳細ID。</param>
        List<ProcessDetailConnection> GetConnectionsByFromId(int fromProcessDetailId);

        /// <summary>
        /// 新しい工程詳細接続情報を追加します。
        /// </summary>
        /// <param name="connection">追加するProcessDetailConnectionオブジェクト。</param>
        void AddProcessDetailConnection(ProcessDetailConnection connection);

        /// <summary>
        /// 指定されたIDの工程詳細接続情報を削除します。
        /// </summary>
        /// <param name="id">削除対象の接続ID。</param>
        void DeleteProcessDetailConnection(int id);

        /// <summary>
        /// 指定されたFromIdとToIdの組み合わせの接続情報を削除します。
        /// </summary>
        /// <param name="fromId">始点の工程詳細ID。</param>
        /// <param name="toId">終点の工程詳細ID。</param>
        void DeleteConnectionsByFromAndTo(int fromId, int toId);

        /// <summary>
        /// 指定されたサイクルIDに紐づく工程詳細終了情報を取得します。
        /// </summary>
        /// <param name="cycleId">取得対象のサイクルID。</param>
        List<ProcessDetailFinish> GetProcessDetailFinishes(int cycleId);
        List<ProcessDetailFinish> GetAllProcessDetailFinishes();

        /// <summary>
        /// 指定されたProcessDetailIdに紐づく終了情報を取得します。
        /// </summary>
        /// <param name="processDetailId">工程詳細ID。</param>
        List<ProcessDetailFinish> GetFinishesByProcessDetailId(int processDetailId);

        /// <summary>
        /// 指定されたFinishProcessDetailIdに紐づく終了情報を取得します。
        /// </summary>
        /// <param name="finishProcessDetailId">終了先の工程詳細ID。</param>
        List<ProcessDetailFinish> GetFinishesByFinishId(int finishProcessDetailId);

        /// <summary>
        /// 新しい工程詳細終了情報を追加します。
        /// </summary>
        /// <param name="finish">追加するProcessDetailFinishオブジェクト。</param>
        void AddProcessDetailFinish(ProcessDetailFinish finish);

        /// <summary>
        /// 指定されたIDの工程詳細終了情報を削除します。
        /// </summary>
        /// <param name="id">削除対象の終了情報ID。</param>
        void DeleteProcessDetailFinish(int id);

        /// <summary>
        /// 指定されたProcessDetailIdとFinishProcessDetailIdの組み合わせの終了情報を削除します。
        /// </summary>
        /// <param name="processDetailId">工程詳細ID。</param>
        /// <param name="finishProcessDetailId">終了先の工程詳細ID。</param>
        void DeleteFinishesByProcessAndFinish(int processDetailId, int finishProcessDetailId);

        #region ProcessStartCondition Methods

        /// <summary>
        /// 指定されたサイクルIDの全ての工程開始条件を取得します。
        /// </summary>
        /// <param name="cycleId">サイクルID。</param>
        List<ProcessStartCondition> GetProcessStartConditions(int cycleId);

        /// <summary>
        /// 指定された工程IDの開始条件を取得します。
        /// </summary>
        /// <param name="processId">工程ID。</param>
        List<ProcessStartCondition> GetStartConditionsByProcessId(int processId);

        /// <summary>
        /// 新しい工程開始条件を追加します。
        /// </summary>
        /// <param name="condition">追加する開始条件。</param>
        void AddProcessStartCondition(ProcessStartCondition condition);

        /// <summary>
        /// 指定されたIDの工程開始条件を削除します。
        /// </summary>
        /// <param name="id">削除対象のID。</param>
        void DeleteProcessStartCondition(int id);

        /// <summary>
        /// 指定された工程IDの全ての開始条件を削除します。
        /// </summary>
        /// <param name="processId">工程ID。</param>
        void DeleteStartConditionsByProcess(int processId);

        #endregion

        #region ProcessFinishCondition Methods

        /// <summary>
        /// 指定されたサイクルIDの全ての工程終了条件を取得します。
        /// </summary>
        /// <param name="cycleId">サイクルID。</param>
        List<ProcessFinishCondition> GetProcessFinishConditions(int cycleId);

        /// <summary>
        /// 指定された工程IDの終了条件を取得します。
        /// </summary>
        /// <param name="processId">工程ID。</param>
        List<ProcessFinishCondition> GetFinishConditionsByProcessId(int processId);

        /// <summary>
        /// 新しい工程終了条件を追加します。
        /// </summary>
        /// <param name="condition">追加する終了条件。</param>
        void AddProcessFinishCondition(ProcessFinishCondition condition);

        /// <summary>
        /// 指定されたIDの工程終了条件を削除します。
        /// </summary>
        /// <param name="id">削除対象のID。</param>
        void DeleteProcessFinishCondition(int id);

        /// <summary>
        /// 指定された工程IDの全ての終了条件を削除します。
        /// </summary>
        /// <param name="processId">工程ID。</param>
        void DeleteFinishConditionsByProcess(int processId);

        #endregion

        #region CylinderIO Methods
        
        /// <summary>
        /// 指定されたCYに関連付けられたIOのリストを取得
        /// </summary>
        List<CylinderIO> GetCylinderIOs(int cylinderId, int plcId);
        
        /// <summary>
        /// 指定されたIOに関連付けられたCYのリストを取得
        /// </summary>
        List<CylinderIO> GetIOCylinders(string ioAddress, int plcId);
        
        /// <summary>
        /// CYとIOの関連付けを追加
        /// </summary>
        void AddCylinderIOAssociation(int cylinderId, string ioAddress, int plcId, string ioType, string? comment = null);
        
        /// <summary>
        /// CYとIOの関連付けを削除
        /// </summary>
        void RemoveCylinderIOAssociation(int cylinderId, string ioAddress, int plcId);
        
        /// <summary>
        /// 指定されたPLCのすべての関連付けを取得
        /// </summary>
        List<CylinderIO> GetAllCylinderIOAssociations(int plcId);
        
        #endregion

        #region OperationIO Methods
        
        /// <summary>
        /// 指定された操作に関連付けられたIOのリストを取得
        /// </summary>
        List<OperationIO> GetOperationIOs(int operationId);
        
        /// <summary>
        /// 指定されたIOに関連付けられた操作のリストを取得
        /// </summary>
        List<OperationIO> GetIOOperations(string ioAddress, int plcId);
        
        /// <summary>
        /// 操作とIOの関連付けを追加
        /// </summary>
        void AddOperationIOAssociation(int operationId, string ioAddress, int plcId, string ioUsage, string? comment = null);
        
        /// <summary>
        /// 操作とIOの関連付けを削除
        /// </summary>
        void RemoveOperationIOAssociation(int operationId, string ioAddress, int plcId);
        
        /// <summary>
        /// 指定されたPLCのすべての関連付けを取得
        /// </summary>
        List<OperationIO> GetAllOperationIOAssociations(int plcId);
        
        #endregion

        #region Error Methods
        
        /// <summary>
        /// エラーテーブルをクリア
        /// </summary>
        void DeleteErrorTable();
        
        /// <summary>
        /// エラーメッセージを取得
        /// </summary>
        List<ErrorMessage> GetErrorMessages(int mnemonicId);
        
        /// <summary>
        /// エラー情報を取得
        /// </summary>
        List<ProcessError> GetErrors(int plcId, int cycleId, int mnemonicId);
        
        void SaveErrors(List<ProcessError> errors);

        void UpdateErrors(List<ProcessError> errors);

        #endregion

        #region MnemonicDevice Methods

        /// <summary>
        /// ニーモニックデバイスを取得
        /// </summary>
        List<MnemonicDevice> GetMnemonicDevices(int plcId);
        
        /// <summary>
        /// 指定されたニーモニックタイプのデバイスを取得
        /// </summary>
        List<MnemonicDevice> GetMnemonicDevicesByMnemonic(int plcId, int mnemonicId);
        
        /// <summary>
        /// ニーモニックデバイスを削除
        /// </summary>
        void DeleteMnemonicDevice(int plcId, int mnemonicId);
        
        /// <summary>
        /// すべてのニーモニックデバイスを削除
        /// </summary>
        void DeleteAllMnemonicDevices();
        
        /// <summary>
        /// ニーモニックデバイスを保存または更新
        /// </summary>
        void SaveOrUpdateMnemonicDevice(MnemonicDevice device);
        
        #endregion

        #region MnemonicSpeedDevice Methods
        
        /// <summary>
        /// 速度デバイスを取得
        /// </summary>
        List<MnemonicSpeedDevice> GetMnemonicSpeedDevices(int plcId);
        
        /// <summary>
        /// 速度テーブルをクリア
        /// </summary>
        void DeleteSpeedTable();
        
        /// <summary>
        /// 速度デバイスを保存または更新
        /// </summary>
        void SaveOrUpdateMnemonicSpeedDevice(MnemonicSpeedDevice device);
        
        #endregion

        #region ProsTime Methods
        
        /// <summary>
        /// 工程時間情報を取得
        /// </summary>
        List<ProsTime> GetProsTimeByPlcId(int plcId);
        
        /// <summary>
        /// 指定されたニーモニックタイプの工程時間情報を取得
        /// </summary>
        List<ProsTime> GetProsTimeByMnemonicId(int plcId, int mnemonicId);
        
        /// <summary>
        /// 工程時間テーブルをクリア
        /// </summary>
        void DeleteProsTimeTable();
        
        /// <summary>
        /// 工程時間情報を保存または更新
        /// </summary>
        void SaveOrUpdateProsTime(ProsTime prosTime);
        
        /// <summary>
        /// 複数の工程時間情報を一括で保存または更新
        /// </summary>
        void SaveOrUpdateProsTimesBatch(List<ProsTime> prosTimes);
        
        /// <summary>
        /// 工程時間定義情報を取得
        /// </summary>
        List<ProsTimeDefinitions> GetProsTimeDefinitions();
        
        #endregion

        #region Memory Methods
        
        /// <summary>
        /// メモリ情報を取得
        /// </summary>
        List<Memory> GetMemories(int plcId);
        
        /// <summary>
        /// メモリカテゴリを取得
        /// </summary>
        List<MemoryCategory> GetMemoryCategories();
        
        /// <summary>
        /// メモリ情報を保存または更新
        /// </summary>
        void SaveOrUpdateMemory(Memory memory);
        
        /// <summary>
        /// 複数のメモリ情報を一括で保存または更新
        /// </summary>
        void SaveOrUpdateMemoriesBatch(List<Memory> memories);
        
        #endregion

        #region Difinitions Methods
        
        /// <summary>
        /// 定義情報を取得
        /// </summary>
        List<Difinitions> GetDifinitions(string category);

        Difinitions? GetDifinition(string category);


        #endregion

    }
}
