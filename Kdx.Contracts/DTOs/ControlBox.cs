using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("ControlBox")]
    public class ControlBox : BaseModel
    {
        [PrimaryKey("PlcId")]
        [Column("PlcId")]
        public int PlcId { get; set; }

        [PrimaryKey("BoxNumber")]
        [Column("BoxNumber")]
        public int BoxNumber { get; set; }

        [Column("ManualMode")]
        public string ManualMode { get; set; } = string.Empty;

        [Column("ManualButton")]
        public string ManualButton { get; set; } = string.Empty;

        [Column("ErrorReset")]
        public string? ErrorReset { get; set; }
    }
}
