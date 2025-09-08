using Kdx.Contracts.DTOs;
using Kdx.Core.Application;
using System.Text.Json;
using Timer = Kdx.Contracts.DTOs.Timer;

namespace KdxMigrationAPI.Services
{
    public interface ITimerDeviceService
    {
        Task<SaveTimerDevicesResult> SaveProcessDetailTimersAsync(
            List<Timer> timers, 
            List<ProcessDetail> details, 
            int plcId);
        
        Task<List<MnemonicTimerDevice>> GetTimerDevicesAsync(int plcId, int? cycleId = null);
    }

    public class TimerDeviceService : ITimerDeviceService
    {
        private readonly SaveProcessDetailTimerDevicesUseCase _saveUseCase;
        private readonly ILogger<TimerDeviceService> _logger;

        public TimerDeviceService(
            SaveProcessDetailTimerDevicesUseCase saveUseCase,
            ILogger<TimerDeviceService> logger)
        {
            _saveUseCase = saveUseCase;
            _logger = logger;
        }

        public async Task<SaveTimerDevicesResult> SaveProcessDetailTimersAsync(
            List<Timer> timers, 
            List<ProcessDetail> details, 
            int plcId)
        {
            try
            {
                _logger.LogInformation(
                    "Processing timer devices for PLC {PlcId}", plcId);

                // ビジネスルールの検証
                var validationErrors = ValidateInput(timers, details);
                if (validationErrors.Any())
                {
                    return new SaveTimerDevicesResult
                    {
                        Success = false,
                        Errors = validationErrors
                    };
                }

                // ユースケースの実行
                var deviceCount = await _saveUseCase.ExecuteAsync(
                    timers, 
                    details, 
                    plcId);

                _logger.LogInformation(
                    "Successfully saved {DeviceCount} timer devices for PLC {PlcId}", 
                    deviceCount, plcId);

                return new SaveTimerDevicesResult
                {
                    Success = true,
                    DeviceCount = deviceCount,
                    Message = $"Successfully saved {deviceCount} timer devices"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error saving timer devices for PLC {PlcId}", plcId);
                
                return new SaveTimerDevicesResult
                {
                    Success = false,
                    Errors = new[] { ex.Message }
                };
            }
        }

        public async Task<List<MnemonicTimerDevice>> GetTimerDevicesAsync(
            int plcId, 
            int? cycleId = null)
        {
            // TODO: リポジトリから取得する実装
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        private List<string> ValidateInput(
            List<Timer> timers, 
            List<ProcessDetail> details)
        {
            var errors = new List<string>();

            if (timers == null || !timers.Any())
            {
                errors.Add("Timers list is empty or null");
            }

            if (details == null || !details.Any())
            {
                errors.Add("ProcessDetails list is empty or null");
            }

            // タイマーカテゴリーの検証
            var invalidTimers = timers?
                .Where(t => t.TimerCategoryId < 0 || t.TimerCategoryId > 10)
                .ToList();
            
            if (invalidTimers?.Any() == true)
            {
                errors.Add($"Invalid timer categories found: {string.Join(", ", 
                    invalidTimers.Select(t => t.TimerCategoryId).Distinct())}");
            }

            return errors;
        }
    }

    public class SaveTimerDevicesResult
    {
        public bool Success { get; set; }
        public int DeviceCount { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
    }
}