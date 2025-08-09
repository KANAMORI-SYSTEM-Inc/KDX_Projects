using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kdx.Contracts.DTOs
{
    [Table("TimerRecordIds")]
    public class TimerRecordId
    {
        [Key]
        [Column(Order = 0)]
        public int TimerId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int RecordId { get; set; }
    }
}