using Kdx.Contracts.DTOs;
using Kdx.Contracts.Enums;
using Kdx.Core.Domain.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kdx.Core.Domain.Factories
{
    // Kdx.Core/Domain/Factories/MnemonicTimerDeviceFactory.cs
    public static class MnemonicTimerDeviceFactory
    {
        public static MnemonicTimerDevice Create(
            Contracts.DTOs.Timer timer, ProcessDetail detail, int sequenceBase,
            IDeviceOffsetProvider offsets, int plcId)
        {
            var prefix = (timer.TimerCategoryId is 6 or 7) ? "T" : "ST";
            var processTimerDevice = $"{prefix}{sequenceBase + offsets.DeviceStartT}";
            var timerDevice = $"ZR{timer.TimerNum + offsets.TimerStartZR}";

            return new MnemonicTimerDevice
            {
                MnemonicId = (int)MnemonicType.ProcessDetail,
                RecordId = detail.Id,
                TimerId = timer.ID,
                TimerCategoryId = timer.TimerCategoryId,
                ProcessTimerDevice = processTimerDevice,
                TimerDevice = timerDevice,
                PlcId = plcId,
                CycleId = timer.CycleId,
                Comment1 = timer.TimerName
            };
        }
    }

}
