using Kdx.Contracts.DTOs;
using Kdx.Contracts.Interfaces;
using Kdx.Infrastructure.Configuration;
using Kdx.Infrastructure.Repositories;
using System.Diagnostics;
using Timer = Kdx.Contracts.DTOs.Timer;

namespace Kdx.Infrastructure.Adapters
{
    /// <summary>
    /// SupabaseRepositoryをIAccessRepositoryインターフェースに適応させるアダプター
    /// 既存のコードとの互換性を保つため、同期メソッドを提供
    /// </summary>
    public class SupabaseRepositoryAdapter : IAccessRepository
    {
        private readonly ISupabaseRepository _supabaseRepository;
        private readonly SupabaseConfiguration _configuration;

        public SupabaseRepositoryAdapter(ISupabaseRepository supabaseRepository, SupabaseConfiguration configuration)
        {
            _supabaseRepository = supabaseRepository ?? throw new ArgumentNullException(nameof(supabaseRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string ConnectionString => _configuration.Url;

        public List<Company> GetCompanies()
        {
            try
            {
                // Task.Runを使用してUIスレッドのデッドロックを回避
                var result = Task.Run(async () => await _supabaseRepository.GetCompaniesAsync()).GetAwaiter().GetResult();
                System.Diagnostics.Debug.WriteLine($"GetCompanies returned {result?.Count ?? 0} items");
                return result ?? new List<Company>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetCompanies: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                // 空のリストを返してアプリケーションのクラッシュを防ぐ
                return new List<Company>();
            }
        }

        public List<Model> GetModels()
        {
            return Task.Run(async () => await _supabaseRepository.GetModelsAsync()).GetAwaiter().GetResult();
        }

        public List<PLC> GetPLCs()
        {
            return Task.Run(async () => await _supabaseRepository.GetPLCsAsync()).GetAwaiter().GetResult();
        }

        public List<Cycle> GetCycles()
        {
            return Task.Run(async () => await _supabaseRepository.GetCyclesAsync()).GetAwaiter().GetResult();
        }

        public List<CylinderCycle> GetCylinderCyclesByPlcId(int plcId)
        {
            return Task.Run(async () => await _supabaseRepository.GetCylinderCyclesByPlcIdAsync(plcId)).GetAwaiter().GetResult();
        }

        public List<ControlBox> GetControlBoxesByPlcId(int plcId)
        {
            return Task.Run(async () => await _supabaseRepository.GetControlBoxesByPlcIdAsync(plcId)).GetAwaiter().GetResult();
        }

        public List<Kdx.Contracts.DTOs.Process> GetProcesses()
        {
            return Task.Run(async () => await _supabaseRepository.GetProcessesAsync()).GetAwaiter().GetResult();
        }

        public int AddProcess(Kdx.Contracts.DTOs.Process process)
        {
            return Task.Run(async () => await _supabaseRepository.AddProcessAsync(process)).GetAwaiter().GetResult();
        }

        public void UpdateProcess(Kdx.Contracts.DTOs.Process process)
        {
            Task.Run(async () => await _supabaseRepository.UpdateProcessAsync(process)).GetAwaiter().GetResult();
        }

        public List<Machine> GetMachines()
        {
            return Task.Run(async () => await _supabaseRepository.GetMachinesAsync()).GetAwaiter().GetResult();
        }

        public Machine? GetMachineById(int nameId, int driveSubId)
        {
            return Task.Run(async () => await _supabaseRepository.GetMachineByIdAsync(nameId, driveSubId)).GetAwaiter().GetResult();
        }

        public MachineName? GetMachineNameById(int id)
        {
            return Task.Run(async () => await _supabaseRepository.GetMachineNameByIdAsync(id)).GetAwaiter().GetResult();
        }

        public List<DriveMain> GetDriveMains()
        {
            return Task.Run(async () => await _supabaseRepository.GetDriveMainsAsync()).GetAwaiter().GetResult();
        }

        public List<DriveSub> GetDriveSubs()
        {
            return Task.Run(async () => await _supabaseRepository.GetDriveSubsAsync()).GetAwaiter().GetResult();
        }

        public DriveSub? GetDriveSubById(int id)
        {
            return Task.Run(async () => await _supabaseRepository.GetDriveSubByIdAsync(id)).GetAwaiter().GetResult();
        }

        public List<Cylinder> GetCYs()
        {
            return Task.Run(async () => await _supabaseRepository.GetCYsAsync()).GetAwaiter().GetResult();
        }

        public List<Cylinder> GetCyList(int plcId)
        {
            return Task.Run(async () => await _supabaseRepository.GetCyListAsync(plcId)).GetAwaiter().GetResult();
        }

        public Cylinder? GetCYById(int id)
        {
            return Task.Run(async () => await _supabaseRepository.GetCYByIdAsync(id)).GetAwaiter().GetResult();
        }

        public List<Timer> GetTimers()
        {
            return Task.Run(async () => await _supabaseRepository.GetTimersAsync()).GetAwaiter().GetResult();
        }

        public List<Timer> GetTimersByCycleId(int cycleId)
        {
            return Task.Run(async () => await _supabaseRepository.GetTimersByCycleIdAsync(cycleId)).GetAwaiter().GetResult();
        }

        public List<MnemonicTimerDevice> GetTimersByRecordId(int cycleId, int mnemonicId, int recordId)
        {
            return Task.Run(async () => await _supabaseRepository.GetTimersByRecordIdAsync(cycleId, mnemonicId, recordId)).GetAwaiter().GetResult();
        }

        public void AddTimer(Timer timer)
        {
            Task.Run(async () => await _supabaseRepository.AddTimerAsync(timer)).GetAwaiter().GetResult();
        }

        public void UpdateTimer(Timer timer)
        {
            Task.Run(async () => await _supabaseRepository.UpdateTimerAsync(timer)).GetAwaiter().GetResult();
        }

        public void DeleteTimer(int id)
        {
            Task.Run(async () => await _supabaseRepository.DeleteTimerAsync(id)).GetAwaiter().GetResult();
        }

        public List<int> GetTimerRecordIds(int timerId)
        {
            return Task.Run(async () => await _supabaseRepository.GetTimerRecordIdsAsync(timerId)).GetAwaiter().GetResult();
        }

        public void AddTimerRecordId(int timerId, int recordId)
        {
            Task.Run(async () => await _supabaseRepository.AddTimerRecordIdAsync(timerId, recordId)).GetAwaiter().GetResult();
        }

        public void DeleteTimerRecordId(int timerId, int recordId)
        {
            Task.Run(async () => await _supabaseRepository.DeleteTimerRecordIdAsync(timerId, recordId)).GetAwaiter().GetResult();
        }

        public void DeleteAllTimerRecordIds(int timerId)
        {
            Task.Run(async () => await _supabaseRepository.DeleteAllTimerRecordIdsAsync(timerId)).GetAwaiter().GetResult();
        }

        public List<Operation> GetOperations()
        {
            return Task.Run(async () => await _supabaseRepository.GetOperationsAsync()).GetAwaiter().GetResult();
        }

        public List<Operation> GetOperationsByCycleId(int cycleId)
        {
            return Task.Run(async () => await _supabaseRepository.GetOperationsByCycleIdAsync(cycleId)).GetAwaiter().GetResult();
        }

        public Operation? GetOperationById(int id)
        {
            return Task.Run(async () => await _supabaseRepository.GetOperationByIdAsync(id)).GetAwaiter().GetResult();
        }

        public List<Length>? GetLengthByPlcId(int plcId)
        {
            return Task.Run(async () => await _supabaseRepository.GetLengthByPlcIdAsync(plcId)).GetAwaiter().GetResult();
        }

        public int AddOperation(Operation operation)
        {
            return Task.Run(async () => await _supabaseRepository.AddOperationAsync(operation)).GetAwaiter().GetResult();
        }

        public void UpdateOperation(Operation operation)
        {
            Task.Run(async () => await _supabaseRepository.UpdateOperationAsync(operation)).GetAwaiter().GetResult();
        }

        public List<ProcessDetail> GetProcessDetails()
        {
            return Task.Run(async () => await _supabaseRepository.GetProcessDetailsAsync()).GetAwaiter().GetResult();
        }

        public void UpdateProcessDetail(ProcessDetail processDetail)
        {
            Task.Run(async () => await _supabaseRepository.UpdateProcessDetailAsync(processDetail)).GetAwaiter().GetResult();
        }

        public int AddProcessDetail(ProcessDetail processDetail)
        {
            return Task.Run(async () => await _supabaseRepository.AddProcessDetailAsync(processDetail)).GetAwaiter().GetResult();
        }

        public void DeleteProcess(int id)
        {
            Task.Run(async () => await _supabaseRepository.DeleteProcessAsync(id)).GetAwaiter().GetResult();
        }

        public void DeleteProcessDetail(int id)
        {
            Task.Run(async () => await _supabaseRepository.DeleteProcessDetailAsync(id)).GetAwaiter().GetResult();
        }

        public void DeleteOperation(int id)
        {
            Task.Run(async () => await _supabaseRepository.DeleteOperationAsync(id)).GetAwaiter().GetResult();
        }

        public List<ProcessCategory> GetProcessCategories()
        {
            return Task.Run(async () => await _supabaseRepository.GetProcessCategoriesAsync()).GetAwaiter().GetResult();
        }

        public List<ProcessDetailCategory> GetProcessDetailCategories()
        {
            return Task.Run(async () => await _supabaseRepository.GetProcessDetailCategoriesAsync()).GetAwaiter().GetResult();
        }

        public ProcessDetailCategory? GetProcessDetailCategoryById(int id)
        {
            return Task.Run(async () => await _supabaseRepository.GetProcessDetailCategoryByIdAsync(id)).GetAwaiter().GetResult();
        }

        public List<OperationCategory> GetOperationCategories()
        {
            return Task.Run(async () => await _supabaseRepository.GetOperationCategoriesAsync()).GetAwaiter().GetResult();
        }

        public List<MnemonicTimerDevice> GetMnemonicTimerDevices()
        {
            return Task.Run(async () => await _supabaseRepository.GetMnemonicTimerDevicesAsync()).GetAwaiter().GetResult();
        }

        public List<MnemonicTimerDevice> GetMnemonicTimerDevicesByCycleId(int plcId, int cycleId)
        {
            return Task.Run(async () => await _supabaseRepository.GetMnemonicTimerDevicesByClcleIdAsync(plcId, cycleId)).GetAwaiter().GetResult();
        }

        public List<MnemonicTimerDevice> GetMnemonicTimerDevicesByCycleAndMnemonicId(int plcId, int cycleId, int mnemonicId)
        {
            return Task.Run(async () => await _supabaseRepository.GetMnemonicTimerDevicesByCycleAndMnemonicIdAsync(plcId, cycleId, mnemonicId)).GetAwaiter().GetResult();
        }

        public List<MnemonicTimerDevice> GetMnemonicTimerDevicesByMnemonicId(int plcId, int mnemonicId)
        {
            Debug.WriteLine($"[SupabaseRepositoryAdapter.GetMnemonicTimerDevicesByMnemonicId] 開始 - plcId: {plcId}, mnemonicId: {mnemonicId}");
            var result = Task.Run(async () => await _supabaseRepository.GetMnemonicTimerDevicesByMnemonicIdAsync(plcId, mnemonicId)).GetAwaiter().GetResult();
            Debug.WriteLine($"[SupabaseRepositoryAdapter.GetMnemonicTimerDevicesByMnemonicId] 完了 - {result.Count}件");
            return result;
        }

        public List<MnemonicTimerDevice> GetMnemonicTimerDevicesByTimerId(int plcId, int timerId)
        {
            return Task.Run(async () => await _supabaseRepository.GetMnemonicTimerDevicesByTimerIdAsync(plcId, timerId)).GetAwaiter().GetResult();
        }

        public void UpdateMnemonicTimerDevice(MnemonicTimerDevice device)
        {
            Task.Run(async () => await _supabaseRepository.UpdateMnemonicTimerDeviceAsync(device)).GetAwaiter().GetResult();
        }

        public void DeleteAllMnemonicTimerDevices()
        {
            Task.Run(async () => await _supabaseRepository.DeleteAllMnemonicTimerDeviceAsync()).GetAwaiter().GetResult();
        }

        public void DeleteMnemonicTimerDevice(int mnemonicId, int recordId, int timerId)
        {
            Task.Run(async () => await _supabaseRepository.DeleteMnemonicTimerDeviceAsync(mnemonicId, recordId, timerId)).GetAwaiter().GetResult();
        }

        public void AddMnemonicTimerDevice(MnemonicTimerDevice device)
        {
            Task.Run(async () => await _supabaseRepository.AddMnemonicTimerDeviceAsync(device)).GetAwaiter().GetResult();
        }

        public void UpsertMnemonicTimerDevice(MnemonicTimerDevice device)
        {
            try
            {
                Debug.WriteLine($"[SupabaseRepositoryAdapter.UpsertMnemonicTimerDevice] 開始");
                Task.Run(async () => await _supabaseRepository.UpsertMnemonicTimerDeviceAsync(device)).GetAwaiter().GetResult();
                Debug.WriteLine($"[SupabaseRepositoryAdapter.UpsertMnemonicTimerDevice] 完了");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SupabaseRepositoryAdapter.UpsertMnemonicTimerDevice] エラー: {ex.Message}");
                throw;
            }
        }

        public List<IO> GetIoList()
        {
            return Task.Run(async () => await _supabaseRepository.GetIoListAsync()).GetAwaiter().GetResult();
        }

        public List<TimerCategory> GetTimerCategory()
        {
            return Task.Run(async () => await _supabaseRepository.GetTimerCategoryAsync()).GetAwaiter().GetResult();
        }

        public List<Servo> GetServos(int? plcId, int? cylinderId)
        {
            return Task.Run(async () => await _supabaseRepository.GetServosAsync(plcId, cylinderId)).GetAwaiter().GetResult();
        }

        public void UpdateIoLinkDevices(IEnumerable<IO> ioRecordsToUpdate)
        {
            Task.Run(async () => await _supabaseRepository.UpdateIoLinkDevicesAsync(ioRecordsToUpdate)).GetAwaiter().GetResult();
        }

        public void UpdateAndLogIoChanges(List<IO> iosToUpdate, List<IOHistory> histories)
        {
            Task.Run(async () => await _supabaseRepository.UpdateAndLogIoChangesAsync(iosToUpdate, histories)).GetAwaiter().GetResult();
        }

        public List<ProcessDetailConnection> GetProcessDetailConnections(int cycleId)
        {
            return Task.Run(async () => await _supabaseRepository.GetProcessDetailConnectionsAsync(cycleId)).GetAwaiter().GetResult();
        }

        public List<ProcessDetailConnection> GetAllProcessDetailConnections()
        {
            return Task.Run(async () => await _supabaseRepository.GetAllProcessDetailConnectionsAsync()).GetAwaiter().GetResult();
        }

        public List<ProcessDetailConnection> GetConnectionsByToId(int toProcessDetailId)
        {
            return Task.Run(async () => await _supabaseRepository.GetConnectionsByToIdAsync(toProcessDetailId)).GetAwaiter().GetResult();
        }

        public List<ProcessDetailConnection> GetConnectionsByFromId(int fromProcessDetailId)
        {
            return Task.Run(async () => await _supabaseRepository.GetConnectionsByFromIdAsync(fromProcessDetailId)).GetAwaiter().GetResult();
        }

        public void AddProcessDetailConnection(ProcessDetailConnection connection)
        {
            Task.Run(async () => await _supabaseRepository.AddProcessDetailConnectionAsync(connection)).GetAwaiter().GetResult();
        }

        public void DeleteProcessDetailConnection(int id)
        {
            Task.Run(async () => await _supabaseRepository.DeleteProcessDetailConnectionAsync(id)).GetAwaiter().GetResult();
        }

        public void DeleteConnectionsByFromAndTo(int fromId, int toId)
        {
            Task.Run(async () => await _supabaseRepository.DeleteConnectionsByFromAndToAsync(fromId, toId)).GetAwaiter().GetResult();
        }

        public List<ProcessDetailFinish> GetProcessDetailFinishes(int cycleId)
        {
            return Task.Run(async () => await _supabaseRepository.GetProcessDetailFinishesAsync(cycleId)).GetAwaiter().GetResult();
        }

        public List<ProcessDetailFinish> GetAllProcessDetailFinishes()
        {
            return Task.Run(async () => await _supabaseRepository.GetAllProcessDetailFinishesAsync()).GetAwaiter().GetResult();
        }

        public List<ProcessDetailFinish> GetFinishesByProcessDetailId(int processDetailId)
        {
            return Task.Run(async () => await _supabaseRepository.GetFinishesByProcessDetailIdAsync(processDetailId)).GetAwaiter().GetResult();
        }

        public List<ProcessDetailFinish> GetFinishesByFinishId(int finishProcessDetailId)
        {
            return Task.Run(async () => await _supabaseRepository.GetFinishesByFinishIdAsync(finishProcessDetailId)).GetAwaiter().GetResult();
        }

        public void AddProcessDetailFinish(ProcessDetailFinish finish)
        {
            Task.Run(async () => await _supabaseRepository.AddProcessDetailFinishAsync(finish)).GetAwaiter().GetResult();
        }

        public void DeleteProcessDetailFinish(int id)
        {
            Task.Run(async () => await _supabaseRepository.DeleteProcessDetailFinishAsync(id)).GetAwaiter().GetResult();
        }

        public void DeleteFinishesByProcessAndFinish(int processDetailId, int finishProcessDetailId)
        {
            Task.Run(async () => await _supabaseRepository.DeleteFinishesByProcessAndFinishAsync(processDetailId, finishProcessDetailId)).GetAwaiter().GetResult();
        }

        #region ProcessStartCondition Methods

        public List<ProcessStartCondition> GetProcessStartConditions(int cycleId)
        {
            return Task.Run(async () => await _supabaseRepository.GetProcessStartConditionsAsync(cycleId)).GetAwaiter().GetResult();
        }

        public List<ProcessStartCondition> GetStartConditionsByProcessId(int processId)
        {
            return Task.Run(async () => await _supabaseRepository.GetStartConditionsByProcessIdAsync(processId)).GetAwaiter().GetResult();
        }

        public void AddProcessStartCondition(ProcessStartCondition condition)
        {
            Task.Run(async () => await _supabaseRepository.AddProcessStartConditionAsync(condition)).GetAwaiter().GetResult();
        }

        public void DeleteProcessStartCondition(int id)
        {
            Task.Run(async () => await _supabaseRepository.DeleteProcessStartConditionAsync(id)).GetAwaiter().GetResult();
        }

        public void DeleteStartConditionsByProcess(int processId)
        {
            Task.Run(async () => await _supabaseRepository.DeleteStartConditionsByProcessAsync(processId)).GetAwaiter().GetResult();
        }

        #endregion

        #region ProcessFinishCondition Methods

        public List<ProcessFinishCondition> GetProcessFinishConditions(int cycleId)
        {
            return Task.Run(async () => await _supabaseRepository.GetProcessFinishConditionsAsync(cycleId)).GetAwaiter().GetResult();
        }

        public List<ProcessFinishCondition> GetFinishConditionsByProcessId(int processId)
        {
            return Task.Run(async () => await _supabaseRepository.GetFinishConditionsByProcessIdAsync(processId)).GetAwaiter().GetResult();
        }

        public void AddProcessFinishCondition(ProcessFinishCondition condition)
        {
            Task.Run(async () => await _supabaseRepository.AddProcessFinishConditionAsync(condition)).GetAwaiter().GetResult();
        }

        public void DeleteProcessFinishCondition(int id)
        {
            Task.Run(async () => await _supabaseRepository.DeleteProcessFinishConditionAsync(id)).GetAwaiter().GetResult();
        }

        public void DeleteFinishConditionsByProcess(int processId)
        {
            Task.Run(async () => await _supabaseRepository.DeleteFinishConditionsByProcessAsync(processId)).GetAwaiter().GetResult();
        }

        #endregion

        #region CylinderIO Methods

        public List<CylinderIO> GetCylinderIOs(int cylinderId, int plcId)
        {
            return Task.Run(async () => await _supabaseRepository.GetCylinderIOsAsync(cylinderId, plcId)).GetAwaiter().GetResult();
        }

        public List<CylinderIO> GetIOCylinders(string ioAddress, int plcId)
        {
            return Task.Run(async () => await _supabaseRepository.GetIOCylindersAsync(ioAddress, plcId)).GetAwaiter().GetResult();
        }

        public void AddCylinderIOAssociation(int cylinderId, string ioAddress, int plcId, string ioType, string? comment = null)
        {
            Task.Run(async () => await _supabaseRepository.AddCylinderIOAssociationAsync(cylinderId, ioAddress, plcId, ioType, comment)).GetAwaiter().GetResult();
        }

        public void RemoveCylinderIOAssociation(int cylinderId, string ioAddress, int plcId)
        {
            Task.Run(async () => await _supabaseRepository.RemoveCylinderIOAssociationAsync(cylinderId, ioAddress, plcId)).GetAwaiter().GetResult();
        }

        public List<CylinderIO> GetAllCylinderIOAssociations(int plcId)
        {
            return Task.Run(async () => await _supabaseRepository.GetAllCylinderIOAssociationsAsync(plcId)).GetAwaiter().GetResult();
        }

        #endregion

        #region OperationIO Methods

        public List<OperationIO> GetOperationIOs(int operationId)
        {
            return Task.Run(async () => await _supabaseRepository.GetOperationIOsAsync(operationId)).GetAwaiter().GetResult();
        }

        public List<OperationIO> GetIOOperations(string ioAddress, int plcId)
        {
            return Task.Run(async () => await _supabaseRepository.GetIOOperationsAsync(ioAddress, plcId)).GetAwaiter().GetResult();
        }

        public void AddOperationIOAssociation(int operationId, string ioAddress, int plcId, string ioUsage, string? comment = null)
        {
            Task.Run(async () => await _supabaseRepository.AddOperationIOAssociationAsync(operationId, ioAddress, plcId, ioUsage, comment)).GetAwaiter().GetResult();
        }

        public void RemoveOperationIOAssociation(int operationId, string ioAddress, int plcId)
        {
            Task.Run(async () => await _supabaseRepository.RemoveOperationIOAssociationAsync(operationId, ioAddress, plcId)).GetAwaiter().GetResult();
        }

        public List<OperationIO> GetAllOperationIOAssociations(int plcId)
        {
            return Task.Run(async () => await _supabaseRepository.GetAllOperationIOAssociationsAsync(plcId)).GetAwaiter().GetResult();
        }

        #endregion

        #region Error Methods

        public void DeleteErrorTable()
        {
            Task.Run(async () => await _supabaseRepository.DeleteErrorTableAsync()).GetAwaiter().GetResult();
        }

        public List<ErrorMessage> GetErrorMessages(int mnemonicId)
        {
            return Task.Run(async () => await _supabaseRepository.GetErrorMessagesAsync(mnemonicId)).GetAwaiter().GetResult();
        }

        public List<ProcessError> GetErrors(int plcId, int cycleId, int mnemonicId)
        {
            return Task.Run(async () => await _supabaseRepository.GetErrorsAsync(plcId, cycleId, mnemonicId)).GetAwaiter().GetResult();
        }

        public void SaveErrors(List<ProcessError> errors)
        {
            Task.Run(async () => await _supabaseRepository.SaveErrorsAsync(errors)).GetAwaiter().GetResult();
        }

        public void UpdateErrors(List<ProcessError> errors)
        {
            Task.Run(async () => await _supabaseRepository.UpdateErrorsAsync(errors)).GetAwaiter().GetResult();
        }

        #endregion

        #region MnemonicDevice Methods

        public List<MnemonicDevice> GetMnemonicDevices(int plcId)
        {
            return Task.Run(async () => await _supabaseRepository.GetMnemonicDevicesAsync(plcId)).GetAwaiter().GetResult();
        }

        public List<MnemonicDevice> GetMnemonicDevicesByMnemonic(int plcId, int mnemonicId)
        {
            return Task.Run(async () => await _supabaseRepository.GetMnemonicDevicesByMnemonicAsync(plcId, mnemonicId)).GetAwaiter().GetResult();
        }

        public void DeleteMnemonicDevice(int plcId, int mnemonicId)
        {
            Task.Run(async () => await _supabaseRepository.DeleteMnemonicDeviceAsync(plcId, mnemonicId)).GetAwaiter().GetResult();
        }

        public void DeleteAllMnemonicDevices()
        {
            Task.Run(async () => await _supabaseRepository.DeleteAllMnemonicDevicesAsync()).GetAwaiter().GetResult();
        }

        public void SaveOrUpdateMnemonicDevice(MnemonicDevice device)
        {
            Task.Run(async () => await _supabaseRepository.SaveOrUpdateMnemonicDeviceAsync(device)).GetAwaiter().GetResult();
        }

        #endregion

        #region MnemonicSpeedDevice Methods

        public List<MnemonicSpeedDevice> GetMnemonicSpeedDevices(int plcId)
        {
            return Task.Run(async () => await _supabaseRepository.GetMnemonicSpeedDevicesAsync(plcId)).GetAwaiter().GetResult();
        }

        public void DeleteSpeedTable()
        {
            Task.Run(async () => await _supabaseRepository.DeleteSpeedTableAsync()).GetAwaiter().GetResult();
        }

        public void SaveOrUpdateMnemonicSpeedDevice(MnemonicSpeedDevice device)
        {
            Task.Run(async () => await _supabaseRepository.SaveOrUpdateMnemonicSpeedDeviceAsync(device)).GetAwaiter().GetResult();
        }

        #endregion

        #region ProsTime Methods

        public List<ProsTime> GetProsTimeByPlcId(int plcId)
        {
            return Task.Run(async () => await _supabaseRepository.GetProsTimeByPlcIdAsync(plcId)).GetAwaiter().GetResult();
        }

        public List<ProsTime> GetProsTimeByMnemonicId(int plcId, int mnemonicId)
        {
            return Task.Run(async () => await _supabaseRepository.GetProsTimeByMnemonicIdAsync(plcId, mnemonicId)).GetAwaiter().GetResult();
        }

        public void DeleteProsTimeTable()
        {
            Task.Run(async () => await _supabaseRepository.DeleteProsTimeTableAsync()).GetAwaiter().GetResult();
        }

        public void SaveOrUpdateProsTime(ProsTime prosTime)
        {
            Task.Run(async () => await _supabaseRepository.SaveOrUpdateProsTimeAsync(prosTime)).GetAwaiter().GetResult();
        }

        public void SaveOrUpdateProsTimesBatch(List<ProsTime> prosTimes)
        {
            Task.Run(async () => await _supabaseRepository.SaveOrUpdateProsTimesBatchAsync(prosTimes)).GetAwaiter().GetResult();
        }

        public List<ProsTimeDefinitions> GetProsTimeDefinitions()
        {
            return Task.Run(async () => await _supabaseRepository.GetProsTimeDefinitionsAsync()).GetAwaiter().GetResult();
        }

        #endregion

        #region Memory Methods

        public List<Memory> GetMemories(int plcId)
        {
            return Task.Run(async () => await _supabaseRepository.GetMemoriesAsync(plcId)).GetAwaiter().GetResult();
        }

        public List<MemoryCategory> GetMemoryCategories()
        {
            return Task.Run(async () => await _supabaseRepository.GetMemoryCategoriesAsync()).GetAwaiter().GetResult();
        }

        public void SaveOrUpdateMemory(Memory memory)
        {
            Task.Run(async () => await _supabaseRepository.SaveOrUpdateMemoryAsync(memory)).GetAwaiter().GetResult();
        }

        public void SaveOrUpdateMemoriesBatch(List<Memory> memories)
        {
            Task.Run(async () => await _supabaseRepository.SaveOrUpdateMemoriesBatchAsync(memories)).GetAwaiter().GetResult();
        }

        #endregion

        #region Difinitions Methods

        public List<Difinitions> GetDifinitions(string category)
        {
            return Task.Run(async () => await _supabaseRepository.GetDifinitionsAsync(category)).GetAwaiter().GetResult();
        }

        public Difinitions? GetDifinition(string category)
        {
            return Task.Run(async () => await _supabaseRepository.GetDifinitionAsync(category)).GetAwaiter().GetResult();
        }

        #endregion
    }
}
