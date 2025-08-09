using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kdx.Contracts.DTOs
{
    [Table("Company")]
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? CreatedAt { get; set; }
    }
}