using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("InterlockIO")]
    public class InterlockIO : BaseModel
    {
        [PrimaryKey("PlcId")]
        [Column("PlcId")]
        public int PlcId { get; set; }

        [Column("IOAddress")]
        public string? IOAddress { get; set; }

        [Column("InterlockConditionId")]
        public int InterlockConditionId { get; set; }

        [Column("IsOnCondition")]
        public bool IsOnCondition { get; set; } = false;

        [Column("ConditionUniqueKey")]
        public int ConditionUniqueKey { get; set; }

    }
}
