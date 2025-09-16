using Kdx.Contracts.DTOs;
using Supabase;
using System.Diagnostics;
using Timer = Kdx.Contracts.DTOs.Timer;

namespace Kdx.Infrastructure.Repositories
{
    /// <summary>
    /// Supabaseデータベースへのアクセス機能を提供するリポジトリの実装。
    /// </summary>
    public class SupabaseRepository : ISupabaseRepository
    {
        private readonly Client _supabaseClient;

        public SupabaseRepository(Client supabaseClient)
        {
            _supabaseClient = supabaseClient ?? throw new ArgumentNullException(nameof(supabaseClient));
        }

        public async Task<List<Company>> GetCompaniesAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Calling Supabase API for Companies...");

                // テーブルの存在とRLS設定を確認
                var response = await _supabaseClient
                    .From<Company>()
                    .Select("*")
                    .Get();

                System.Diagnostics.Debug.WriteLine($"Supabase returned {response?.Models?.Count ?? 0} companies");

                var companies = response?.Models?.ToList() ?? new List<Company>();

                // データが0件の場合、テストデータを挿入してみる
                if (companies.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("No companies found. Attempting to insert test data...");
                    try
                    {
                        var testCompany = new Company
                        {
                            CompanyName = "Test Company",
                            CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        };

                        var insertResponse = await _supabaseClient
                            .From<Company>()
                            .Insert(testCompany);

                        System.Diagnostics.Debug.WriteLine("Test company inserted successfully");

                        // 挿入後に再度取得
                        response = await _supabaseClient
                            .From<Company>()
                            .Select("*")
                            .Get();

                        companies = response?.Models?.ToList() ?? new List<Company>();
                        System.Diagnostics.Debug.WriteLine($"After insert, found {companies.Count} companies");
                    }
                    catch (Exception insertEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to insert test data: {insertEx.Message}");
                        System.Diagnostics.Debug.WriteLine($"This might indicate RLS is enabled. Check Supabase dashboard.");
                    }
                }

                return companies;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Supabase API error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<List<Model>> GetModelsAsync()
        {
            var response = await _supabaseClient
                .From<Model>()
                .Get();
            return response.Models;
        }

        public async Task<List<PLC>> GetPLCsAsync()
        {
            var response = await _supabaseClient
                .From<PLC>()
                .Get();
            return response.Models;
        }

        public async Task<List<Cycle>> GetCyclesAsync()
        {
            var response = await _supabaseClient
                .From<Cycle>()
                .Get();
            return response.Models;
        }

        public async Task<List<CylinderCycle>> GetCylinderCyclesByPlcIdAsync(int plcId)
        {
            var response = await _supabaseClient
                .From<CylinderCycle>()
                .Where(c => c.PlcId == plcId)
                .Get();
            return response.Models;
        }

        public async Task<List<ControlBox>> GetControlBoxesByPlcIdAsync(int plcId)
        {
            var response = await _supabaseClient
                .From<ControlBox>()
                .Where(c => c.PlcId == plcId)
                .Get();
            return response.Models;
        }

        public async Task<List<Kdx.Contracts.DTOs.Process>> GetProcessesAsync()
        {
            var response = await _supabaseClient
                .From<Kdx.Contracts.DTOs.Process>()
                .Get();
            return response.Models;
        }

        public async Task<int> AddProcessAsync(Kdx.Contracts.DTOs.Process process)
        {
            var response = await _supabaseClient
                .From<Kdx.Contracts.DTOs.Process>()
                .Insert(process);
            return response.Models.FirstOrDefault()?.Id ?? 0;
        }

        public async Task UpdateProcessAsync(Kdx.Contracts.DTOs.Process process)
        {
            await _supabaseClient
                .From<Kdx.Contracts.DTOs.Process>()
                .Where(p => p.Id == process.Id)
                .Update(process);
        }

        public async Task<List<Machine>> GetMachinesAsync()
        {
            var response = await _supabaseClient
                .From<Machine>()
                .Get();
            return response.Models;
        }

        public async Task<Machine?> GetMachineByIdAsync(int nameId, int driveSubId)
        {
            var response = await _supabaseClient
                .From<Machine>()
                .Where(m => m.MacineNameId == nameId && m.DriveSubId == driveSubId)
                .Single();
            return response;
        }

        public async Task<MachineName?> GetMachineNameByIdAsync(int id)
        {
            var response = await _supabaseClient
                .From<MachineName>()
                .Where(m => m.Id == id)
                .Single();
            return response;
        }

        public async Task<List<DriveMain>> GetDriveMainsAsync()
        {
            var response = await _supabaseClient
                .From<DriveMain>()
                .Get();
            return response.Models;
        }

        public async Task<List<DriveSub>> GetDriveSubsAsync()
        {
            var response = await _supabaseClient
                .From<DriveSub>()
                .Get();
            return response.Models;
        }

        public async Task<DriveSub?> GetDriveSubByIdAsync(int id)
        {
            var response = await _supabaseClient
                .From<DriveSub>()
                .Where(d => d.Id == id)
                .Single();
            return response;
        }

        public async Task<List<Cylinder>> GetCYsAsync()
        {
            var response = await _supabaseClient
                .From<Cylinder>()
                .Get();
            return response.Models;
        }

        public async Task<List<Cylinder>> GetCyListAsync(int plcId)
        {
            var response = await _supabaseClient
                .From<Cylinder>()
                .Where(c => c.PlcId == plcId)
                .Get();
            return response.Models;
        }

        public async Task<Cylinder?> GetCYByIdAsync(int id)
        {
            var response = await _supabaseClient
                .From<Cylinder>()
                .Where(c => c.Id == id)
                .Single();
            return response;
        }

        public async Task<List<Timer>> GetTimersAsync()
        {
            var response = await _supabaseClient
                .From<Timer>()
                .Get();
            return response.Models;
        }

        public async Task<List<Timer>> GetTimersByCycleIdAsync(int cycleId)
        {
            var response = await _supabaseClient
                .From<Timer>()
                .Where(t => t.CycleId == cycleId)
                .Get();
            return response.Models;
        }

        public async Task<List<MnemonicTimerDevice>> GetTimersByRecordIdAsync(int cycleId, int mnemonicId, int recordId)
        {
            var response = await _supabaseClient
                .From<MnemonicTimerDevice>()
                .Where(t => t.MnemonicId == mnemonicId && t.RecordId == recordId)
                .Get();
            return response.Models;
        }

        public async Task AddTimerAsync(Timer timer)
        {
            await _supabaseClient
                .From<Timer>()
                .Insert(timer);
        }

        public async Task UpdateTimerAsync(Timer timer)
        {
            await _supabaseClient
                .From<Timer>()
                .Where(t => t.ID == timer.ID)
                .Update(timer);
        }

        public async Task DeleteTimerAsync(int id)
        {
            await _supabaseClient
                .From<Timer>()
                .Where(t => t.ID == id)
                .Delete();
        }

        public async Task<List<int>> GetTimerRecordIdsAsync(int timerId)
        {
            try
            {

                var response = await _supabaseClient
                    .From<TimerRecordId>()
                    .Where(t => t.TimerId == timerId)
                    .Get();

                var recordIds = response?.Models?.Select(t => t.RecordId).ToList() ?? new List<int>();

                return recordIds;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetTimerRecordIdsAsync] エラー: {ex.Message}");
                return new List<int>();
            }
        }

        public async Task AddTimerRecordIdAsync(int timerId, int recordId)
        {
            // TimerRecordId関連テーブルが必要な場合は実装を追加
            var timerRecord = new TimerRecordId
            {
                TimerId = timerId,
                RecordId = recordId
            };
            await _supabaseClient
                .From<TimerRecordId>()
                .Insert(timerRecord);
        }

        public async Task DeleteTimerRecordIdAsync(int timerId, int recordId)
        {
            // TimerRecordId関連テーブルが必要な場合は実装を追加
            await _supabaseClient
                .From<TimerRecordId>()
                .Where(t => t.TimerId == timerId && t.RecordId == recordId)
                .Delete();
        }

        public async Task DeleteAllTimerRecordIdsAsync(int timerId)
        {
            // TimerRecordId関連テーブルが必要な場合は実装を追加
            await _supabaseClient
                .From<TimerRecordId>()
                .Where(t => t.TimerId == timerId)
                .Delete();
        }

        public async Task<List<Operation>> GetOperationsAsync()
        {
            var response = await _supabaseClient
                .From<Operation>()
                .Get();
            return response.Models;
        }

        public async Task<List<Operation>> GetOperationsByCycleIdAsync(int cycleId)
        {
            var response = await _supabaseClient
                .From<Operation>()
                .Where(o => o.CycleId == cycleId)
                .Get();
            return response.Models;
        }

        public async Task<Operation?> GetOperationByIdAsync(int id)
        {
            var response = await _supabaseClient
                .From<Operation>()
                .Where(o => o.Id == id)
                .Single();
            return response;
        }

        public async Task<List<Length>?> GetLengthByPlcIdAsync(int plcId)
        {
            var response = await _supabaseClient
                .From<Length>()
                .Where(l => l.PlcId == plcId)
                .Get();
            return response.Models;
        }

        public async Task<int> AddOperationAsync(Operation operation)
        {
            var response = await _supabaseClient
                .From<Operation>()
                .Insert(operation);
            return response.Models.FirstOrDefault()?.Id ?? 0;
        }

        public async Task UpdateOperationAsync(Operation operation)
        {
            await _supabaseClient
                .From<Operation>()
                .Where(o => o.Id == operation.Id)
                .Update(operation);
        }

        public async Task<List<ProcessDetail>> GetProcessDetailsAsync()
        {
            var response = await _supabaseClient
                .From<ProcessDetail>()
                .Get();
            return response.Models;
        }

        public async Task UpdateProcessDetailAsync(ProcessDetail processDetail)
        {
            await _supabaseClient
                .From<ProcessDetail>()
                .Where(p => p.Id == processDetail.Id)
                .Update(processDetail);
        }

        public async Task<int> AddProcessDetailAsync(ProcessDetail processDetail)
        {
            var response = await _supabaseClient
                .From<ProcessDetail>()
                .Insert(processDetail);
            return response.Models.FirstOrDefault()?.Id ?? 0;
        }

        public async Task DeleteProcessAsync(int id)
        {
            await _supabaseClient
                .From<Kdx.Contracts.DTOs.Process>()
                .Where(p => p.Id == id)
                .Delete();
        }

        public async Task DeleteProcessDetailAsync(int id)
        {
            await _supabaseClient
                .From<ProcessDetail>()
                .Where(p => p.Id == id)
                .Delete();
        }

        public async Task DeleteOperationAsync(int id)
        {
            await _supabaseClient
                .From<Operation>()
                .Where(o => o.Id == id)
                .Delete();
        }

        public async Task<List<ProcessCategory>> GetProcessCategoriesAsync()
        {
            var response = await _supabaseClient
                .From<ProcessCategory>()
                .Get();
            return response.Models;
        }

        public async Task<List<ProcessDetailCategory>> GetProcessDetailCategoriesAsync()
        {
            var response = await _supabaseClient
                .From<ProcessDetailCategory>()
                .Get();
            return response.Models;
        }

        public async Task<ProcessDetailCategory?> GetProcessDetailCategoryByIdAsync(int id)
        {
            var response = await _supabaseClient
                .From<ProcessDetailCategory>()
                .Where(p => p.Id == id)
                .Single();
            return response;
        }

        public async Task<List<OperationCategory>> GetOperationCategoriesAsync()
        {
            var response = await _supabaseClient
                .From<OperationCategory>()
                .Get();
            return response.Models;
        }

        public async Task<List<MnemonicTimerDevice>> GetMnemonicTimerDevicesAsync()
        {
            var response = await _supabaseClient
                .From<MnemonicTimerDevice>()
                .Get();
            return response.Models;
        }

        public async Task<List<MnemonicTimerDevice>> GetMnemonicTimerDevicesByClcleIdAsync(int plcId, int cycleId)
        {
            var response = await _supabaseClient
                .From<MnemonicTimerDevice>()
                .Where(m => m.PlcId == plcId && m.CycleId == cycleId)
                .Get();
            return response.Models;
        }

        public async Task<List<MnemonicTimerDevice>> GetMnemonicTimerDevicesByCycleAndMnemonicIdAsync(int plcId, int cycleId, int mnemonicId)
        {
            var response = await _supabaseClient
                .From<MnemonicTimerDevice>()
                .Where(m => m.PlcId == plcId && m.CycleId == cycleId && m.MnemonicId == mnemonicId)
                .Get();
            return response.Models;
        }

        public async Task<List<MnemonicTimerDevice>> GetMnemonicTimerDevicesByMnemonicIdAsync(int plcId, int mnemonicId)
        {
            try
            {
                var response = await _supabaseClient
                    .From<MnemonicTimerDevice>()
                    .Where(m => m.PlcId == plcId && m.MnemonicId == mnemonicId)
                    .Get();
                return response?.Models ?? new List<MnemonicTimerDevice>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetMnemonicTimerDevicesByMnemonicIdAsync] エラー: {ex.Message}");
                return new List<MnemonicTimerDevice>();
            }
        }

        public async Task<List<MnemonicTimerDevice>> GetMnemonicTimerDevicesByTimerIdAsync(int plcId, int timerId)
        {
            var response = await _supabaseClient
                .From<MnemonicTimerDevice>()
                .Where(m => m.PlcId == plcId && m.TimerId == timerId)
                .Get();
            return response.Models;
        }

        public async Task UpdateMnemonicTimerDeviceAsync(MnemonicTimerDevice device)
        {
            await _supabaseClient
                .From<MnemonicTimerDevice>()
                .Where(m => m.MnemonicId == device.MnemonicId &&
                           m.RecordId == device.RecordId &&
                           m.TimerId == device.TimerId)
                .Update(device);
        }

        public async Task DeleteAllMnemonicTimerDeviceAsync()
        {
            // Supabase requires a WHERE clause for DELETE. To delete all records, we use a condition that's always true
            await _supabaseClient
                .From<MnemonicTimerDevice>()
                .Filter("PlcId", Postgrest.Constants.Operator.GreaterThanOrEqual, "0")
                .Delete();
        }

        public async Task DeleteMnemonicTimerDeviceAsync(int mnemonicId, int recordId, int timerId)
        {
            await _supabaseClient
                .From<MnemonicTimerDevice>()
                .Where(m => m.MnemonicId == mnemonicId &&
                           m.RecordId == recordId &&
                           m.TimerId == timerId)
                .Delete();
        }

        public async Task AddMnemonicTimerDeviceAsync(MnemonicTimerDevice device)
        {
            await _supabaseClient
                .From<MnemonicTimerDevice>()
                .Insert(device);
        }

        public async Task UpsertMnemonicTimerDeviceAsync(MnemonicTimerDevice device)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[UpsertMnemonicTimerDeviceAsync] 開始");
                System.Diagnostics.Debug.WriteLine($"  MnemonicId: {device.MnemonicId}, RecordId: {device.RecordId}, TimerId: {device.TimerId}");
                System.Diagnostics.Debug.WriteLine($"  PlcId: {device.PlcId}, CycleId: {device.CycleId}");
                System.Diagnostics.Debug.WriteLine($"  TimerDeviceT: {device.TimerDeviceT}, TimerDeviceZR: {device.TimerDeviceZR}");
                System.Diagnostics.Debug.WriteLine($"  Comment1: {device.Comment1}");

                // Supabaseのupsert機能を使用
                // upsertは自動的に主キーで競合を検出して更新または挿入を行う
                var response = await _supabaseClient
                    .From<MnemonicTimerDevice>()
                    .Upsert(device);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UpsertMnemonicTimerDeviceAsync] エラー発生: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"  スタックトレース: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"  内部エラー: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public async Task<List<IO>> GetIoListAsync()
        {
            var allIOs = new List<IO>();
            const int pageSize = 1000;
            int offset = 0;
            bool hasMore = true;

            while (hasMore)
            {
                var response = await _supabaseClient
                    .From<IO>()
                    .Range(offset, offset + pageSize - 1)
                    .Get();

                if (response?.Models != null && response.Models.Any())
                {
                    allIOs.AddRange(response.Models);
                    offset += pageSize;
                    hasMore = response.Models.Count == pageSize;
                }
                else
                {
                    hasMore = false;
                }
            }

            return allIOs;
        }

        public async Task<List<TimerCategory>> GetTimerCategoryAsync()
        {
            var response = await _supabaseClient
                .From<TimerCategory>()
                .Get();
            return response.Models;
        }

        public async Task<List<Servo>> GetServosAsync(int? plcId, int? cylinderId)
        {
            var query = (Postgrest.Table<Servo>)_supabaseClient.From<Servo>();

            if (plcId.HasValue)
                query = query.Where(s => s.PlcId == plcId.Value);

            if (cylinderId.HasValue)
                query = query.Where(s => s.CylinderId == cylinderId.Value);

            var response = await query.Get();
            return response.Models;
        }

        public async Task UpdateIoLinkDevicesAsync(IEnumerable<IO> ioRecordsToUpdate)
        {
            // バッチ更新を実行
            foreach (var io in ioRecordsToUpdate)
            {
                await _supabaseClient
                    .From<IO>()
                    .Where(i => i.Address == io.Address && i.PlcId == io.PlcId)
                    .Update(io);
            }
        }

        public async Task UpdateAndLogIoChangesAsync(List<IO> iosToUpdate, List<IOHistory> histories)
        {
            // トランザクション処理が必要な場合は、Supabaseの関数を使用するか、
            // 個別の処理として実装
            foreach (var io in iosToUpdate)
            {
                await _supabaseClient
                    .From<IO>()
                    .Where(i => i.Address == io.Address && i.PlcId == io.PlcId)
                    .Update(io);
            }

            foreach (var history in histories)
            {
                await _supabaseClient
                    .From<IOHistory>()
                    .Insert(history);
            }
        }

        public async Task<List<ProcessDetailConnection>> GetProcessDetailConnectionsAsync(int cycleId)
        {
            // ProcessDetailConnection doesn't have CycleId column in the database
            // We need to get connections through ProcessDetail relationships
            var processDetails = await _supabaseClient
                .From<ProcessDetail>()
                .Where(pd => pd.CycleId == cycleId)
                .Get();

            if (!processDetails.Models.Any())
                return new List<ProcessDetailConnection>();

            var processDetailIds = processDetails.Models.Select(pd => pd.Id).ToList();

            // Get connections where either From or To ProcessDetailId is in our list
            var connections = new List<ProcessDetailConnection>();

            // Get connections FROM our process details
            foreach (var pdId in processDetailIds)
            {
                var fromConnections = await _supabaseClient
                    .From<ProcessDetailConnection>()
                    .Where(c => c.FromProcessDetailId == pdId)
                    .Get();
                connections.AddRange(fromConnections.Models);
            }

            // Get connections TO our process details
            foreach (var pdId in processDetailIds)
            {
                var toConnections = await _supabaseClient
                    .From<ProcessDetailConnection>()
                    .Where(c => c.ToProcessDetailId == pdId)
                    .Get();
                connections.AddRange(toConnections.Models);
            }

            // Remove duplicates and return
            return connections.Distinct().ToList();
        }

        public async Task<List<ProcessDetailConnection>> GetAllProcessDetailConnectionsAsync()
        {
            var response = await _supabaseClient
                .From<ProcessDetailConnection>()
                .Get();
            return response.Models;
        }

        public async Task<List<ProcessDetailConnection>> GetConnectionsByToIdAsync(int toProcessDetailId)
        {
            var response = await _supabaseClient
                .From<ProcessDetailConnection>()
                .Where(p => p.ToProcessDetailId == toProcessDetailId)
                .Get();
            return response.Models;
        }

        public async Task<List<ProcessDetailConnection>> GetConnectionsByFromIdAsync(int fromProcessDetailId)
        {
            var response = await _supabaseClient
                .From<ProcessDetailConnection>()
                .Where(p => p.FromProcessDetailId == fromProcessDetailId)
                .Get();
            return response.Models;
        }

        public async Task AddProcessDetailConnectionAsync(ProcessDetailConnection connection)
        {
            await _supabaseClient
                .From<ProcessDetailConnection>()
                .Insert(connection);
        }

        public async Task DeleteProcessDetailConnectionAsync(int id)
        {
            await _supabaseClient
                .From<ProcessDetailConnection>()
                .Where(p => p.Id == id)
                .Delete();
        }

        public async Task DeleteConnectionsByFromAndToAsync(int fromId, int toId)
        {
            await _supabaseClient
                .From<ProcessDetailConnection>()
                .Where(p => p.FromProcessDetailId == fromId && p.ToProcessDetailId == toId)
                .Delete();
        }

        public async Task<List<ProcessDetailFinish>> GetProcessDetailFinishesAsync(int cycleId)
        {
            // ProcessDetailFinishテーブルにはCycleIdがないため、
            // ProcessDetailテーブルから該当するCycleIdのProcessDetailIdを取得してフィルタリング
            var processDetails = await _supabaseClient
                .From<ProcessDetail>()
                .Where(pd => pd.CycleId == cycleId)
                .Get();

            if (!processDetails.Models.Any())
                return new List<ProcessDetailFinish>();

            var processDetailIds = processDetails.Models.Select(pd => pd.Id).ToList();
            var finishes = new List<ProcessDetailFinish>();

            // ProcessDetailIdごとにFinishを取得
            foreach (var processDetailId in processDetailIds)
            {
                var response = await _supabaseClient
                    .From<ProcessDetailFinish>()
                    .Where(f => f.ProcessDetailId == processDetailId)
                    .Get();

                finishes.AddRange(response.Models);
            }

            return finishes;
        }

        public async Task<List<ProcessDetailFinish>> GetAllProcessDetailFinishesAsync()
        {
            var response = await _supabaseClient
                .From<ProcessDetailFinish>()
                .Get();
            return response.Models;
        }

        public async Task<List<ProcessDetailFinish>> GetFinishesByProcessDetailIdAsync(int processDetailId)
        {
            var response = await _supabaseClient
                .From<ProcessDetailFinish>()
                .Where(p => p.ProcessDetailId == processDetailId)
                .Get();
            return response.Models;
        }

        public async Task<List<ProcessDetailFinish>> GetFinishesByFinishIdAsync(int finishProcessDetailId)
        {
            var response = await _supabaseClient
                .From<ProcessDetailFinish>()
                .Where(p => p.FinishProcessDetailId == finishProcessDetailId)
                .Get();
            return response.Models;
        }

        public async Task AddProcessDetailFinishAsync(ProcessDetailFinish finish)
        {
            await _supabaseClient
                .From<ProcessDetailFinish>()
                .Insert(finish);
        }

        public async Task DeleteProcessDetailFinishAsync(int id)
        {
            await _supabaseClient
                .From<ProcessDetailFinish>()
                .Where(p => p.Id == id)
                .Delete();
        }

        public async Task DeleteFinishesByProcessAndFinishAsync(int processDetailId, int finishProcessDetailId)
        {
            await _supabaseClient
                .From<ProcessDetailFinish>()
                .Where(p => p.ProcessDetailId == processDetailId &&
                           p.FinishProcessDetailId == finishProcessDetailId)
                .Delete();
        }

        #region ProcessStartCondition Methods

        public async Task<List<ProcessStartCondition>> GetProcessStartConditionsAsync(int cycleId)
        {
            // ProcessStartConditionテーブルにはCycleIdがないため、
            // まずProcessテーブルから該当するCycleIdのProcessIdを取得し、
            // それらのProcessIdに関連するStartConditionを取得する
            var processes = await _supabaseClient
                .From<Kdx.Contracts.DTOs.Process>()
                .Where(p => p.CycleId == cycleId)
                .Get();

            if (!processes.Models.Any())
                return new List<ProcessStartCondition>();

            var processIds = processes.Models.Select(p => p.Id).ToList();
            var conditions = new List<ProcessStartCondition>();

            // ProcessIdごとにStartConditionを取得
            foreach (var processId in processIds)
            {
                var response = await _supabaseClient
                    .From<ProcessStartCondition>()
                    .Where(c => c.ProcessId == processId)
                    .Get();

                conditions.AddRange(response.Models);
            }

            return conditions;
        }

        public async Task<List<ProcessStartCondition>> GetStartConditionsByProcessIdAsync(int processId)
        {
            var response = await _supabaseClient
                .From<ProcessStartCondition>()
                .Where(p => p.ProcessId == processId)
                .Get();
            return response.Models;
        }

        public async Task AddProcessStartConditionAsync(ProcessStartCondition condition)
        {
            await _supabaseClient
                .From<ProcessStartCondition>()
                .Insert(condition);
        }

        public async Task DeleteProcessStartConditionAsync(int id)
        {
            await _supabaseClient
                .From<ProcessStartCondition>()
                .Where(p => p.Id == id)
                .Delete();
        }

        public async Task DeleteStartConditionsByProcessAsync(int processId)
        {
            await _supabaseClient
                .From<ProcessStartCondition>()
                .Where(p => p.ProcessId == processId)
                .Delete();
        }

        #endregion

        #region ProcessFinishCondition Methods

        public async Task<List<ProcessFinishCondition>> GetProcessFinishConditionsAsync(int cycleId)
        {
            // ProcessFinishConditionテーブルにもCycleIdがないため、
            // ProcessStartConditionと同様の処理を行う
            var processes = await _supabaseClient
                .From<Kdx.Contracts.DTOs.Process>()
                .Where(p => p.CycleId == cycleId)
                .Get();

            if (!processes.Models.Any())
                return new List<ProcessFinishCondition>();

            var processIds = processes.Models.Select(p => p.Id).ToList();
            var conditions = new List<ProcessFinishCondition>();

            // ProcessIdごとにFinishConditionを取得
            foreach (var processId in processIds)
            {
                var response = await _supabaseClient
                    .From<ProcessFinishCondition>()
                    .Where(c => c.ProcessId == processId)
                    .Get();

                conditions.AddRange(response.Models);
            }

            return conditions;
        }

        public async Task<List<ProcessFinishCondition>> GetFinishConditionsByProcessIdAsync(int processId)
        {
            var response = await _supabaseClient
                .From<ProcessFinishCondition>()
                .Where(p => p.ProcessId == processId)
                .Get();
            return response.Models;
        }

        public async Task AddProcessFinishConditionAsync(ProcessFinishCondition condition)
        {
            await _supabaseClient
                .From<ProcessFinishCondition>()
                .Insert(condition);
        }

        public async Task DeleteProcessFinishConditionAsync(int id)
        {
            await _supabaseClient
                .From<ProcessFinishCondition>()
                .Where(p => p.Id == id)
                .Delete();
        }

        public async Task DeleteFinishConditionsByProcessAsync(int processId)
        {
            await _supabaseClient
                .From<ProcessFinishCondition>()
                .Where(p => p.ProcessId == processId)
                .Delete();
        }

        #endregion

        #region CylinderIO Methods

        public async Task<List<CylinderIO>> GetCylinderIOsAsync(int cylinderId, int plcId)
        {
            var response = await _supabaseClient
                .From<CylinderIO>()
                .Where(c => c.CylinderId == cylinderId && c.PlcId == plcId)
                .Get();
            return response.Models;
        }

        public async Task<List<CylinderIO>> GetIOCylindersAsync(string ioAddress, int plcId)
        {
            var response = await _supabaseClient
                .From<CylinderIO>()
                .Where(c => c.Address == ioAddress && c.PlcId == plcId)
                .Get();
            return response.Models;
        }

        public async Task AddCylinderIOAssociationAsync(int cylinderId, string ioAddress, int plcId, string ioType, string? comment = null)
        {
            var cylinderIO = new CylinderIO
            {
                CylinderId = cylinderId,
                Address = ioAddress,
                PlcId = plcId,
                IOType = ioType,
                Comment = comment
            };
            await _supabaseClient.From<CylinderIO>().Insert(cylinderIO);
        }

        public async Task RemoveCylinderIOAssociationAsync(int cylinderId, string ioAddress, int plcId)
        {
            await _supabaseClient
                .From<CylinderIO>()
                .Where(c => c.CylinderId == cylinderId && c.Address == ioAddress && c.PlcId == plcId)
                .Delete();
        }

        public async Task<List<CylinderIO>> GetAllCylinderIOAssociationsAsync(int plcId)
        {
            var response = await _supabaseClient
                .From<CylinderIO>()
                .Where(c => c.PlcId == plcId)
                .Get();
            return response.Models;
        }

        #endregion

        #region OperationIO Methods

        public async Task<List<OperationIO>> GetOperationIOsAsync(int operationId)
        {
            var response = await _supabaseClient
                .From<OperationIO>()
                .Where(o => o.OperationId == operationId)
                .Get();
            return response.Models;
        }

        public async Task<List<OperationIO>> GetIOOperationsAsync(string ioAddress, int plcId)
        {
            var response = await _supabaseClient
                .From<OperationIO>()
                .Where(o => o.Address == ioAddress && o.PlcId == plcId)
                .Get();
            return response.Models;
        }

        public async Task AddOperationIOAssociationAsync(int operationId, string ioAddress, int plcId, string ioUsage, string? comment = null)
        {
            var operationIO = new OperationIO
            {
                OperationId = operationId,
                Address = ioAddress,
                PlcId = plcId,
                Usage = ioUsage,
                Comment = comment
            };
            await _supabaseClient.From<OperationIO>().Insert(operationIO);
        }

        public async Task RemoveOperationIOAssociationAsync(int operationId, string ioAddress, int plcId)
        {
            await _supabaseClient
                .From<OperationIO>()
                .Where(o => o.OperationId == operationId && o.Address == ioAddress && o.PlcId == plcId)
                .Delete();
        }

        public async Task<List<OperationIO>> GetAllOperationIOAssociationsAsync(int plcId)
        {
            var response = await _supabaseClient
                .From<OperationIO>()
                .Where(o => o.PlcId == plcId)
                .Get();
            return response.Models;
        }

        #endregion

        #region Error Methods

        public async Task DeleteErrorTableAsync()
        {
            await _supabaseClient
                .From<ProcessError>()
                .Filter("PlcId", Postgrest.Constants.Operator.GreaterThanOrEqual, "0") // 全削除のための条件
                .Delete();
        }

        public async Task<List<ErrorMessage>> GetErrorMessagesAsync(int mnemonicId)
        {
            var response = await _supabaseClient
                .From<ErrorMessage>()
                .Where(e => e.MnemonicId == mnemonicId)
                .Get();
            return response.Models;
        }

        public async Task<List<ProcessError>> GetErrorsAsync(int plcId, int cycleId, int mnemonicId)
        {
            var response = await _supabaseClient
                .From<ProcessError>()
                .Where(e => e.PlcId == plcId)
                .Where(e => e.CycleId == cycleId)
                .Where(e => e.MnemonicId == mnemonicId)
                .Get();
            return response.Models;
        }

        public async Task SaveErrorsAsync(List<ProcessError> errors)
        {
            if (errors == null || !errors.Any()) return;

            var errorsToInsert = errors.Select(e => new ProcessError
            {
                PlcId = e.PlcId,
                CycleId = e.CycleId,
                Device = e.Device,
                MnemonicId = e.MnemonicId,
                RecordId = e.RecordId,
                AlarmId = e.AlarmId,
                ErrorNum = e.ErrorNum,
                Comment1 = e.Comment1,
                Comment2 = e.Comment2,
                Comment3 = e.Comment3,
                Comment4 = e.Comment4,
                AlarmComment = e.AlarmComment,
                MessageComment = e.MessageComment,
                ErrorTime = e.ErrorTime,
                ErrorTimeDevice = e.ErrorTimeDevice
            }).ToList();

            await _supabaseClient
                .From<ProcessError>()
                .Insert(errorsToInsert);  // 一括挿入
        }



        public async Task UpdateErrorsAsync(List<ProcessError> errors)
        {
            if (errors == null || !errors.Any())
                return;
            foreach (var error in errors)
            {
                await _supabaseClient
                    .From<ProcessError>()
                    .Where(e => e.PlcId == error.PlcId && e.AlarmId == error.AlarmId)
                    .Update(error);
            }
        }


        #endregion

        #region MnemonicDevice Methods

        public async Task<List<MnemonicDevice>> GetMnemonicDevicesAsync(int plcId)
        {
            var response = await _supabaseClient
                .From<MnemonicDevice>()
                .Where(m => m.PlcId == plcId)
                .Get();
            return response.Models;
        }

        public async Task<List<MnemonicDevice>> GetMnemonicDevicesByMnemonicAsync(int plcId, int mnemonicId)
        {
            var response = await _supabaseClient
                .From<MnemonicDevice>()
                .Where(m => m.PlcId == plcId && m.MnemonicId == mnemonicId)
                .Get();
            return response.Models;
        }

        public async Task DeleteMnemonicDeviceAsync(int plcId, int mnemonicId)
        {
            await _supabaseClient
                .From<MnemonicDevice>()
                .Where(m => m.PlcId == plcId && m.MnemonicId == mnemonicId)
                .Delete();
        }

        public async Task DeleteAllMnemonicDevicesAsync()
        {
            // Supabase requires a WHERE clause for DELETE. To delete all records, we use a condition that's always true
            await _supabaseClient
                .From<MnemonicDevice>()
                .Filter("PlcId", Postgrest.Constants.Operator.GreaterThanOrEqual, "0")
                .Delete();
        }

        public async Task SaveOrUpdateMnemonicDeviceAsync(MnemonicDevice device)
        {
            // Supabaseのupsert機能を使用（PlcId, MnemonicId, RecordIdの複合主キーに基づく）
            await _supabaseClient
                .From<MnemonicDevice>()
                .Upsert(device);
        }

        #endregion

        #region MnemonicSpeedDevice Methods

        public async Task<List<MnemonicSpeedDevice>> GetMnemonicSpeedDevicesAsync(int plcId)
        {
            var response = await _supabaseClient
                .From<MnemonicSpeedDevice>()
                .Where(m => m.PlcId == plcId)
                .Get();
            return response.Models;
        }

        public async Task DeleteSpeedTableAsync()
        {
            // Supabase requires a WHERE clause for DELETE. To delete all records, we use a condition that's always true
            await _supabaseClient
                .From<MnemonicSpeedDevice>()
                .Filter("ID", Postgrest.Constants.Operator.GreaterThanOrEqual, "0")
                .Delete();
        }

        public async Task SaveOrUpdateMnemonicSpeedDeviceAsync(MnemonicSpeedDevice device)
        {
            // Supabaseのupsert機能を使用
            await _supabaseClient
                .From<MnemonicSpeedDevice>()
                .Upsert(device);
        }

        #endregion

        #region ProsTime Methods

        public async Task<List<ProsTime>> GetProsTimeByPlcIdAsync(int plcId)
        {
            var response = await _supabaseClient
                .From<ProsTime>()
                .Where(p => p.PlcId == plcId)
                .Get();
            return response.Models;
        }

        public async Task<List<ProsTime>> GetProsTimeByMnemonicIdAsync(int plcId, int mnemonicId)
        {
            try
            {

                var response = await _supabaseClient
                    .From<ProsTime>()
                    .Where(p => p.PlcId == plcId && p.MnemonicId == mnemonicId)
                    .Get();

                var prosTimes = response?.Models ?? new List<ProsTime>();

                return prosTimes;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[GetProsTimeByMnemonicIdAsync] エラー: {ex.Message}");
                return new List<ProsTime>();
            }
        }

        public async Task DeleteProsTimeTableAsync()
        {
            // Supabase requires a WHERE clause for DELETE. To delete all records, we use a condition that's always true
            await _supabaseClient
                .From<ProsTime>()
                .Filter("SortId", Postgrest.Constants.Operator.GreaterThanOrEqual, "0")
                .Delete();
        }

        public async Task SaveOrUpdateProsTimeAsync(ProsTime prosTime)
        {
            // Supabaseのupsert機能を使用（PlcId, MnemonicId, RecordId, SortIdの複合主キーに基づく）
            await _supabaseClient
                .From<ProsTime>()
                .Upsert(prosTime);
        }

        public async Task SaveOrUpdateProsTimesBatchAsync(List<ProsTime> prosTimes)
        {
            if (prosTimes == null || !prosTimes.Any())
                return;

            // Supabaseのupsert機能を使用して一括更新
            await _supabaseClient
                .From<ProsTime>()
                .Upsert(prosTimes);
        }

        public async Task<List<ProsTimeDefinitions>> GetProsTimeDefinitionsAsync()
        {
            try
            {
                var response = await _supabaseClient
                    .From<ProsTimeDefinitions>()
                    .Get();

                return response?.Models ?? new List<ProsTimeDefinitions>();
            }
            catch (Exception ex)
            {
                // エラー: {ex.Message}
                Debug.WriteLine($"[GetProsTimeDefinitionsAsync] エラー: {ex.Message}");
                return new List<ProsTimeDefinitions>();
            }
        }

        #endregion

        #region Memory Methods

        public async Task<List<Memory>> GetMemoriesAsync(int plcId)
        {
            var response = await _supabaseClient
                .From<Memory>()
                .Where(m => m.PlcId == plcId)
                .Get();
            return response.Models;
        }

        public async Task<List<MemoryCategory>> GetMemoryCategoriesAsync()
        {
            var response = await _supabaseClient
                .From<MemoryCategory>()
                .Get();
            return response.Models;
        }

        public async Task SaveOrUpdateMemoryAsync(Memory memory)
        {
            // Supabaseのupsert機能を使用（PlcIdとDeviceの複合主キーに基づく）
            await _supabaseClient
                .From<Memory>()
                .Upsert(memory);
        }

        public async Task SaveOrUpdateMemoriesBatchAsync(List<Memory> memories)
        {
            if (memories == null || !memories.Any())
                return;

            // Supabaseのupsert機能を使用して一括更新（PlcIdとDeviceの複合主キーに基づく）
            // Supabaseは複数レコードの一括upsertをサポート
            await _supabaseClient
                .From<Memory>()
                .Upsert(memories);
        }

        #endregion

        #region Difinitions Methods

        public async Task<List<Difinitions>> GetDifinitionsAsync(string category)
        {
            var response = await _supabaseClient
                .From<Difinitions>()
                .Where(d => d.Category == category)
                .Get();
            return response.Models;
        }

        public async Task<Difinitions?> GetDifinitionAsync(string category)
        {
            var response = await _supabaseClient
                .From<Difinitions>()
                .Where(d => d.Category == category)
                .Single();

            return response;
        }

        #endregion
    }
}
