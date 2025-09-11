using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("ProcessDetailFinish")]
    public class ProcessDetailFinish : BaseModel
    {
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }
        
        [Postgrest.Attributes.Column("CycleId")]
        public int? CycleId { get; set; }
        
        [Postgrest.Attributes.Column("ProcessDetailId")]
        public int ProcessDetailId { get; set; }
        
        [Postgrest.Attributes.Column("FinishProcessDetailId")]
        public int? FinishProcessDetailId { get; set; }
        
        [Postgrest.Attributes.Column("FinishProcessId")]
        public int? FinishProcessId { get; set; }
        
        [Postgrest.Attributes.Column("FinishSensor")]
        public string? FinishSensor { get; set; }
    }
}