using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("Timer")]
    public class Timer : BaseModel
    {
        [PrimaryKey("ID")]
        [Column("ID")]
        public int ID { get; set; }

        [Column("CycleId")]
        public int? CycleId { get; set; }

        [Column("TimerCategoryId")]
        public int? TimerCategoryId { get; set; }

        [Column("TimerNum")]
        public int? TimerNum { get; set; }

        [Column("TimerName")]
        public string? TimerName { get; set; }

        [Column("MnemonicId")]
        public int? MnemonicId { get; set; }

        [Column("Example")]
        public int? Example { get; set; }
    }
}
