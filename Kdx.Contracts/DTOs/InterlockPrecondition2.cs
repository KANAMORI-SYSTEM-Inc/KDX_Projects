using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("InterlockPrecondition2")]
    public class InterlockPrecondition2 : BaseModel
    {
        [PrimaryKey("Id")]
        [Column("Id")]
        public int Id { get; set; }

        [Column("IsEnableProcess")]
        public bool IsEnableProcess { get; set; } = false;

        [Column("InterlockMode")]
        public string? InterlockMode { get; set; }

        [Column("StartDetailId")]
        public int? StartDetailId { get; set; }

        [Column("EndDetailId")]
        public int? EndDetailId { get; set; }

    }
}
