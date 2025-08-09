using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kdx.Contracts.DTOs
{
    [Table("CY")]
    public class OperationBlock
    {
        [Key]
        public int Id { get; set; }
        public string? OperationBlockName { get; set; }


    }

}