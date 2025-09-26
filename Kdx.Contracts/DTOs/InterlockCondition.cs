using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("InterlockCondition")]
    public class InterlockCondition : BaseModel
    {
        [PrimaryKey("Id")]
        [Column("Id")]
        public int Id { get; set; }

        [Column("InterlockId")]
        public int InterlockId { get; set; }

        [Column("ConditionNumber")]
        public int ConditionNumber { get; set; }

        [Column("ConditionTypeId")]
        public int ConditionTypeId { get; set; }

    }
}
