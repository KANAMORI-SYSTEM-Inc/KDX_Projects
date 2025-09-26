using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("Interlock")]
    public class Interlock : BaseModel
    {
        [PrimaryKey("Id")]
        [Column("Id")]
        public int Id { get; set; }

        [Column("PlcId")]
        public int PlcId { get; set; }

        [Column("CylinderId")]
        public int CylinderId { get; set; }

        [Column("SortId")]
        public int SortId { get; set; }

        [Column("ConditionCylinderId")]
        public int ConditionCylinderId { get; set; }
    }
}
