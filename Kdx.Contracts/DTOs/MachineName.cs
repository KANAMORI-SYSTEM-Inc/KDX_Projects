using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("MachineName")]
    public class MachineName : BaseModel
    {
        [PrimaryKey("Id")]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FullName")]
        public string FullName { get; set; } = string.Empty;

        [Column("ShortName")]
        public string ShortName { get; set; } = string.Empty;

    }
}
