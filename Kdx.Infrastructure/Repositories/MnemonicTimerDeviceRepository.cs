using Dapper;
using Kdx.Contracts.DTOs;
using Kdx.Core.Domain.Services;
using Kdx.Core.Domain.Interfaces;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading;
using System.Threading.Tasks;

namespace Kdx.Infrastructure.Repositories
{
    public sealed class MnemonicTimerDeviceRepository : IMnemonicTimerDeviceRepository
    {
        private readonly IAccessRepository _accessRepository;
        private readonly string _connectionString;

        public MnemonicTimerDeviceRepository(IAccessRepository accessRepository)
        {
            _accessRepository = accessRepository;
            _connectionString = accessRepository.GetConnectionString();
        }

        public async Task AddRangeAsync(IEnumerable<MnemonicTimerDevice> devices, CancellationToken ct)
        {
            using var connection = new OleDbConnection(_connectionString);
            await connection.OpenAsync(ct);
            
            using var transaction = connection.BeginTransaction();
            try
            {
                const string sql = @"
                    INSERT INTO MnemonicTimerDevice (
                        MnemonicId, RecordId, TimerId, TimerCategoryId,
                        ProcessTimerDevice, TimerDevice, PlcId, CycleId, Comment1
                    ) VALUES (
                        @MnemonicId, @RecordId, @TimerId, @TimerCategoryId,
                        @ProcessTimerDevice, @TimerDevice, @PlcId, @CycleId, @Comment1
                    )";

                foreach (var device in devices)
                {
                    await connection.ExecuteAsync(sql, device, transaction);
                    if (ct.IsCancellationRequested)
                    {
                        transaction.Rollback();
                        ct.ThrowIfCancellationRequested();
                    }
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
