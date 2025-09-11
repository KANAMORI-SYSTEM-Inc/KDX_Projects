using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("ProcessDetailConnection")]
    public class ProcessDetailConnection : BaseModel
    {
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }
        
        // Note: CycleId doesn't exist in the database table
        // Use ProcessDetail.CycleId to filter connections by cycle
        
        [Postgrest.Attributes.Column("FromProcessDetailId")]
        public int FromProcessDetailId { get; set; }
        
        [Postgrest.Attributes.Column("ToProcessDetailId")]
        public int? ToProcessDetailId { get; set; }
        
        [Postgrest.Attributes.Column("ToProcessId")]
        public int? ToProcessId { get; set; }
        
        [Postgrest.Attributes.Column("StartSensor")]
        public string? StartSensor { get; set; }
    }
}