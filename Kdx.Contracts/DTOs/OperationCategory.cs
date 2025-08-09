using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kdx.Contracts.DTOs
{
    [Table("OperationCategory")]
    public class OperationCategory
    {
        [Key]
        public int Id { get; set; }
        public string? CategoryName { get; set; }
    }
}