using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kdx.Contracts.DTOs
{
    [Table("Place")]
    public class Place
    {
        [Key]
        public int Id { get; set; }
        public int? ModelId { get; set; }
        public string? PlaceName { get; set; }
    }
}