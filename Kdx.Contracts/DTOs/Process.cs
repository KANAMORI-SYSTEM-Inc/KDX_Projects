using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("Process")]
    public class Process : BaseModel
    {
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }
        
        [Postgrest.Attributes.Column("ProcessName")]
        public string? ProcessName { get; set; }
        
        [Postgrest.Attributes.Column("CycleId")]
        public int? CycleId { get; set; }
        
        [Postgrest.Attributes.Column("TestStart")]
        public string? TestStart {  get; set; }
        
        [Postgrest.Attributes.Column("TestCondition")]
        public string? TestCondition { get; set; }
        
        [Postgrest.Attributes.Column("TestMode")]
        public string? TestMode { get; set; }
        
        [Postgrest.Attributes.Column("AutoMode")]
        public string? AutoMode { get; set; }
        
        [Postgrest.Attributes.Column("AutoStart")]
        public string? AutoStart { get; set; }
        
        [Postgrest.Attributes.Column("ProcessCategoryId")]
        public int? ProcessCategoryId { get; set; }
        
        [Postgrest.Attributes.Column("ILStart")]
        public string? ILStart { get; set; }
        
        [Postgrest.Attributes.Column("Comment1")]
        public string? Comment1 { get; set; }
        
        [Postgrest.Attributes.Column("Comment2")]
        public string? Comment2 { get; set; }
        
        [Postgrest.Attributes.Column("SortNumber")]
        public int? SortNumber { get; set; }

    }
}