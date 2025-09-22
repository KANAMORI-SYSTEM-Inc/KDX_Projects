using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("OperationCategory")]
    public class OperationCategory : BaseModel
    {
        [PrimaryKey("Id")]
        [Column("Id")]
        public int Id { get; set; }
        [Column("CategoryName")]
        public string CategoryName { get; set; } = string.Empty;
        [Column("CategoryType")]
        public string CategoryType { get; set; } = string.Empty;
    }
}
