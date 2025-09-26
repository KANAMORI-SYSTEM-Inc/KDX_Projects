using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("InterlockPrecondition1")]
    public class InterlockPrecondition1 : BaseModel
    {
        [PrimaryKey("Id")]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Discription")]
        public string? Discription { get; set; }

        [Column("ConditionName")]
        public string? ConditionName { get; set; }

    }
}
