using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("InterlockConditionType")]
    public class InterlockConditionType : BaseModel
    {
        [PrimaryKey("Id")]
        [Column("Id")]
        public int Id { get; set; }

        [Column("ConditionTypeName")]
        public string? ConditionTypeName { get; set; }

        [Column("Discription")]
        public string? Discription { get; set; }

        [Column("NeedIOSearch")]
        public bool NeedIOSearch { get; set; } = false;

    }
}
