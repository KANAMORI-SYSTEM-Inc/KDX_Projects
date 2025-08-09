using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kdx.Contracts.DTOs
{
    [Table("ProcessCategory")]
    public class ProcessCategory
    {
        [Key]
        public int Id { get; set; }
        public string? ProcessCategoryName { get; set; }

    }
}