using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("Memory")]
    public class Memory : BaseModel
    {
        [PrimaryKey("PlcId")]
        [Postgrest.Attributes.Column("PlcId")]
        public int PlcId { get; set; }
        
        [PrimaryKey("Device")]
        [Postgrest.Attributes.Column("Device")]
        public string Device { get; set; } = string.Empty;
        
        [Postgrest.Attributes.Column("MemoryCategory")]
        public int? MemoryCategory { get; set; }
        
        [Postgrest.Attributes.Column("DeviceNumber")]
        public int? DeviceNumber { get; set; }
        
        [Postgrest.Attributes.Column("DeviceNumber1")]
        public string? DeviceNumber1 { get; set; }
        
        [Postgrest.Attributes.Column("DeviceNumber2")]
        public string? DeviceNumber2 { get; set; }
        
        [Postgrest.Attributes.Column("Category")]
        public string? Category { get; set; }
        
        [Postgrest.Attributes.Column("Row_1")]
        public string? Row_1 { get; set; }
        
        [Postgrest.Attributes.Column("Row_2")]
        public string? Row_2 { get; set; }
        
        [Postgrest.Attributes.Column("Row_3")]
        public string? Row_3 { get; set; }
        
        [Postgrest.Attributes.Column("Row_4")]
        public string? Row_4 { get; set; }
        
        [Postgrest.Attributes.Column("Direct_Input")]
        public string? Direct_Input { get; set; }
        
        [Postgrest.Attributes.Column("Confirm")]
        public string? Confirm { get; set; }
        
        [Postgrest.Attributes.Column("Note")]
        public string? Note { get; set; }
        
        [Postgrest.Attributes.Column("GOT")]
        public string? GOT { get; set; }
        
        [Postgrest.Attributes.Column("MnemonicId")]
        public int? MnemonicId { get; set; }
        
        [Postgrest.Attributes.Column("RecordId")]
        public int? RecordId { get; set; }
        
        [Postgrest.Attributes.Column("OutcoilNumber")]
        public int? OutcoilNumber { get; set; }

    }
}