using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("CylinderCycle")]
    public class CylinderCycle : BaseModel
    {
        [PrimaryKey("CylinderId")]
        [Column("CylinderId")]
        public int CylinderId { get; set; }

        [Column("PlcId")]
        public int PlcId { get; set; }

        [PrimaryKey("CylinderId")]
        [Column("CycleId")]
        public int CycleId { get; set; }
    }
}
