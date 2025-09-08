using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kdx.Contracts.DTOs
{
    [Table("ProcessDetailConnection")]
    public class ProcessDetailConnection
    {
        [Key]
        public int Id { get; set; }
        
        public int FromProcessDetailId { get; set; }
        
        public int? ToProcessDetailId { get; set; }
        
        public int? ToProcessId { get; set; }
        
        public string? StartSensor { get; set; }
    }
}