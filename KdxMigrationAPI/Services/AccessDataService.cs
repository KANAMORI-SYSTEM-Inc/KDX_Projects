using Dapper;
using System.Data.OleDb;
using KdxMigrationAPI.Models;

namespace KdxMigrationAPI.Services
{
    /// <summary>
    /// Accessデータベースからデータを読み込むサービス
    /// </summary>
    public class AccessDataService
    {
        private readonly string _connectionString;
        private readonly ILogger<AccessDataService> _logger;

        public AccessDataService(IConfiguration configuration, ILogger<AccessDataService> logger)
        {
            _connectionString = configuration.GetConnectionString("AccessDatabase") 
                ?? throw new InvalidOperationException("Access database connection string not configured");
            _logger = logger;
        }

        /// <summary>
        /// AccessデータベースからIOテーブルのデータを取得
        /// </summary>
        public async Task<IEnumerable<IO>> GetIODataFromAccessAsync()
        {
            try
            {
                using var connection = new OleDbConnection(_connectionString);
                await connection.OpenAsync();

                var sql = @"
                    SELECT 
                        Address,
                        PlcId,
                        Name,
                        IOType,
                        IsInverted,
                        IsEnabled
                    FROM IO
                    ORDER BY Address, PlcId";

                var accessData = await connection.QueryAsync<dynamic>(sql);
                
                var ioList = new List<IO>();
                foreach (var row in accessData)
                {
                    ioList.Add(new IO
                    {
                        Address = row.Address?.ToString() ?? string.Empty,
                        PlcId = Convert.ToInt32(row.PlcId ?? 0),
                        Name = row.Name?.ToString(),
                        IOType = Convert.ToInt32(row.IOType ?? 0),
                        IsInverted = Convert.ToBoolean(row.IsInverted ?? false),
                        IsEnabled = Convert.ToBoolean(row.IsEnabled ?? true),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }

                _logger.LogInformation($"Retrieved {ioList.Count} IO records from Access database");
                return ioList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving IO data from Access database");
                throw;
            }
        }

        /// <summary>
        /// Accessデータベースの接続テスト
        /// </summary>
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = new OleDbConnection(_connectionString);
                await connection.OpenAsync();
                _logger.LogInformation("Access database connection successful");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Access database connection failed");
                return false;
            }
        }
    }
}