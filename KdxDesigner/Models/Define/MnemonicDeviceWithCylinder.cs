using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kdx.Contracts.DTOs;

namespace KdxDesigner.Models.Define
{
    public class MnemonicDeviceWithCylinder
    {
        public MnemonicDevice Mnemonic { get; set; } = default!;
        public Cylinder Cylinder { get; set; } = default!;
        
    }
}
