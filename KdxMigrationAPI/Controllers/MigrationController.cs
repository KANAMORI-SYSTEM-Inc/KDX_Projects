using Microsoft.AspNetCore.Mvc;
using KdxMigrationAPI.Services;

namespace KdxMigrationAPI.Controllers
{
    /// <summary>
    /// データ移行API
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class MigrationController : ControllerBase
    {
        private readonly MigrationService _migrationService;
        private readonly AccessDataService _accessDataService;
        private readonly ILogger<MigrationController> _logger;

        public MigrationController(
            MigrationService migrationService,
            AccessDataService accessDataService,
            ILogger<MigrationController> logger)
        {
            _migrationService = migrationService;
            _accessDataService = accessDataService;
            _logger = logger;
        }

        /// <summary>
        /// Accessデータベース接続テスト
        /// </summary>
        [HttpGet("test-connection")]
        public async Task<IActionResult> TestConnection()
        {
            var result = await _accessDataService.TestConnectionAsync();
            return Ok(new { success = result, message = result ? "Connection successful" : "Connection failed" });
        }

        /// <summary>
        /// IOデータの移行実行
        /// </summary>
        /// <param name="clearExisting">既存データをクリアするか</param>
        [HttpPost("migrate-io")]
        public async Task<IActionResult> MigrateIOData([FromQuery] bool clearExisting = false)
        {
            _logger.LogInformation($"Starting IO data migration (clearExisting: {clearExisting})");
            var result = await _migrationService.MigrateIODataAsync(clearExisting);
            
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(500, result);
            }
        }

        /// <summary>
        /// IOデータ統計情報取得
        /// </summary>
        [HttpGet("io-statistics")]
        public async Task<IActionResult> GetIOStatistics()
        {
            var stats = await _migrationService.GetIOStatisticsAsync();
            return Ok(stats);
        }
    }
}