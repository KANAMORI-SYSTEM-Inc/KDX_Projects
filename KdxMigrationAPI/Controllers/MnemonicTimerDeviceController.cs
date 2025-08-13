using Kdx.Contracts.DTOs;
using Kdx.Core.Application;
using Microsoft.AspNetCore.Mvc;

namespace KdxMigrationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MnemonicTimerDeviceController : ControllerBase
    {
        private readonly SaveProcessDetailTimerDevicesUseCase _useCase;
        private readonly ILogger<MnemonicTimerDeviceController> _logger;

        public MnemonicTimerDeviceController(
            SaveProcessDetailTimerDevicesUseCase useCase,
            ILogger<MnemonicTimerDeviceController> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        [HttpPost("save-process-detail-timers")]
        public async Task<IActionResult> SaveProcessDetailTimers(
            [FromBody] SaveTimerDevicesRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Saving timer devices for PLC {PlcId} with {DetailCount} details and {TimerCount} timers",
                    request.PlcId, request.Details.Count, request.Timers.Count);

                var result = await _useCase.ExecuteAsync(
                    request.Timers,
                    request.Details,
                    request.PlcId);

                return Ok(new SaveTimerDevicesResponse
                {
                    Success = true,
                    DeviceCount = result,
                    Message = $"Successfully saved {result} timer devices"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving timer devices");
                return StatusCode(500, new SaveTimerDevicesResponse
                {
                    Success = false,
                    DeviceCount = 0,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { Status = "Healthy", Service = "MnemonicTimerDevice" });
        }
    }

    public class SaveTimerDevicesRequest
    {
        public List<Kdx.Contracts.DTOs.Timer> Timers { get; set; } = new();
        public List<ProcessDetail> Details { get; set; } = new();
        public int PlcId { get; set; }
    }

    public class SaveTimerDevicesResponse
    {
        public bool Success { get; set; }
        public int DeviceCount { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}