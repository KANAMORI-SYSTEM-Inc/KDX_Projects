using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("Cycle")]
    public class Cycle : BaseModel
    {
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }

        [Postgrest.Attributes.Column("PlcId")]
        public int PlcId { get; set; }

        [Postgrest.Attributes.Column("CycleName")]
        public string? CycleName { get; set; }

        [Postgrest.Attributes.Column("StartDevice")]
        public string StartDevice { get; set; } = "L1000";

        [Postgrest.Attributes.Column("ResetDevice")]
        public string ResetDevice { get; set; } = "L1001";

    }
}
