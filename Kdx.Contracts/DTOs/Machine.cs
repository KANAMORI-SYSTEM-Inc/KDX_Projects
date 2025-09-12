using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("Machine")]
    public class Machine : BaseModel
    {
        [PrimaryKey("MachineNameId")]
        [Column("MachineNameId")]
        public int MacineNameId { get; set; }

        [PrimaryKey("DriveSubId")]
        [Column("DriveSubIdId")]
        public int DriveSubId { get; set; }

        [Column("UseValveRetention")]
        public bool UseValveRetention { get; set; } = false;

        [Column("Description")]
        public string? Description { get; set; }
    }
}
