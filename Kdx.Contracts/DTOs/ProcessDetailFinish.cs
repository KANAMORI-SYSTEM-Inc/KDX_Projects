using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kdx.Contracts.DTOs
{
    [Table("ProcessDetailFinish")]
    public class ProcessDetailFinish
    {
        [Key]
        public int Id { get; set; }
        
        public int ProcessDetailId { get; set; }
        
        public int FinishProcessDetailId { get; set; }
        
        public string? FinishSensor { get; set; }
    }
}