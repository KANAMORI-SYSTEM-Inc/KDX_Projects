using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("CylinderControlBox")]
    public class CylinderControlBox : BaseModel
    {
        [PrimaryKey("CylinderId")]
        [Column("CylinderId")]
        public int CylinderId { get; set; }
        [PrimaryKey("PlcId")]
        [Column("PlcId")]
        public int PlcId { get; set; }  // デフォルト値を0に設定
        [PrimaryKey("ManualNumber")]
        [Column("ManualNumber")]
        public int ManualNumber { get; set; }
        [Column("Device")]
        public string? Device { get; set; } = string.Empty;
    }
}
