using System.ComponentModel.DataAnnotations;

namespace Kdx.Contracts.DTOs
{
    public class Machine
    {
        [Key]
        public int Id { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public string ShortName { get; set; } = string.Empty;
    }
}