using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kdx.Contracts.DTOs
{
    [Table("TimerCategory")]
    public class TimerCategory
    {
        [Key]
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryFig { get; set; }
    }
}