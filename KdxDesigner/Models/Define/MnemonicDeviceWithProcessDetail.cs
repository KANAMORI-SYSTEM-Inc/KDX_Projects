using Kdx.Contracts.DTOs;

namespace KdxDesigner.Models.Define
{
    public class MnemonicDeviceWithProcessDetail
    {
        public Kdx.Contracts.DTOs.MnemonicDevice Mnemonic { get; set; } = default!;
        public ProcessDetail Detail { get; set; } = default!;

    }
}
