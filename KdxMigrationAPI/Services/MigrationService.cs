using Microsoft.EntityFrameworkCore;
using KdxMigrationAPI.Data;
using KdxMigrationAPI.Models;

namespace KdxMigrationAPI.Services
{
    /// <summary>
    /// データ移行サービス
    /// </summary>
    public class MigrationService
    {
        private readonly MigrationDbContext _dbContext;
        private readonly AccessDataService _accessDataService;
        private readonly ILogger<MigrationService> _logger;

        public MigrationService(
            MigrationDbContext dbContext,
            AccessDataService accessDataService,
            ILogger<MigrationService> logger)
        {
            _dbContext = dbContext;
            _accessDataService = accessDataService;
            _logger = logger;
        }

        /// <summary>
        /// IOテーブルのデータをAccessからPostgreSQLへ移行
        /// </summary>
        public async Task<MigrationResult> MigrateIODataAsync(bool clearExisting = false)
        {
            var result = new MigrationResult();
            
            try
            {
                // 既存データのクリア（オプション）
                if (clearExisting)
                {
                    _logger.LogInformation("Clearing existing IO data in PostgreSQL");
                    await _dbContext.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"IOs\" RESTART IDENTITY");
                    result.DeletedCount = await _dbContext.IOs.CountAsync();
                }

                // Accessからデータ取得
                _logger.LogInformation("Fetching IO data from Access database");
                var accessData = await _accessDataService.GetIODataFromAccessAsync();
                result.SourceCount = accessData.Count();

                if (!accessData.Any())
                {
                    _logger.LogWarning("No IO data found in Access database");
                    return result;
                }

                // PostgreSQLへの挿入処理
                foreach (var io in accessData)
                {
                    try
                    {
                        // 既存レコードの確認
                        var existing = await _dbContext.IOs
                            .FirstOrDefaultAsync(x => x.Address == io.Address && x.PlcId == io.PlcId);

                        if (existing != null)
                        {
                            // 更新
                            existing.Name = io.Name;
                            existing.IOType = io.IOType;
                            existing.IsInverted = io.IsInverted;
                            existing.IsEnabled = io.IsEnabled;
                            existing.UpdatedAt = DateTime.UtcNow;
                            result.UpdatedCount++;
                        }
                        else
                        {
                            // 新規追加
                            await _dbContext.IOs.AddAsync(io);
                            result.InsertedCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Errors.Add($"Error processing IO {io.Address}-{io.PlcId}: {ex.Message}");
                        _logger.LogError(ex, $"Error processing IO record: {io.Address}-{io.PlcId}");
                    }
                }

                // 変更を保存
                await _dbContext.SaveChangesAsync();
                
                result.Success = true;
                result.Message = $"Migration completed: {result.InsertedCount} inserted, {result.UpdatedCount} updated";
                _logger.LogInformation(result.Message);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Migration failed: {ex.Message}";
                _logger.LogError(ex, "IO data migration failed");
            }

            return result;
        }

        /// <summary>
        /// PostgreSQLのIOデータ統計情報を取得
        /// </summary>
        public async Task<IOStatistics> GetIOStatisticsAsync()
        {
            var stats = new IOStatistics
            {
                TotalCount = await _dbContext.IOs.CountAsync(),
                InputCount = await _dbContext.IOs.Where(x => x.IOType == 0).CountAsync(),
                OutputCount = await _dbContext.IOs.Where(x => x.IOType == 1).CountAsync(),
                EnabledCount = await _dbContext.IOs.Where(x => x.IsEnabled).CountAsync(),
                DisabledCount = await _dbContext.IOs.Where(x => !x.IsEnabled).CountAsync()
            };

            return stats;
        }
    }

    /// <summary>
    /// 移行結果
    /// </summary>
    public class MigrationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int SourceCount { get; set; }
        public int InsertedCount { get; set; }
        public int UpdatedCount { get; set; }
        public int DeletedCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    /// <summary>
    /// IO統計情報
    /// </summary>
    public class IOStatistics
    {
        public int TotalCount { get; set; }
        public int InputCount { get; set; }
        public int OutputCount { get; set; }
        public int EnabledCount { get; set; }
        public int DisabledCount { get; set; }
    }
}