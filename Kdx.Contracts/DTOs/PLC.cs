using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kdx.Contracts.DTOs
{
    [Table("PLC")]
    public class PLC
    {
        [Key]
        public int Id { get; set; }
        public string? PlcName { get; set; }
        public int? ModelId { get; set; }
        public string? Maker { get; set; }
    }
}