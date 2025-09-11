using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("IO")]
    public class IO : BaseModel
    {
        [PrimaryKey("Address")]
        [Postgrest.Attributes.Column("Address")]
        public string Address { get; set; } = string.Empty;
        
        [PrimaryKey("PlcId")]
        [Postgrest.Attributes.Column("PlcId")]
        public int PlcId { get; set; }
        
        // Properties from KdxDesigner version
        [Postgrest.Attributes.Column("IOText")]
        public string? IOText { get; set; }
        
        [Postgrest.Attributes.Column("XComment")]
        public string? XComment { get; set; }
        
        [Postgrest.Attributes.Column("YComment")]
        public string? YComment { get; set; }
        
        [Postgrest.Attributes.Column("FComment")]
        public string? FComment { get; set; }
        
        [Postgrest.Attributes.Column("IOName")]
        public string? IOName { get; set; }
        
        [Postgrest.Attributes.Column("IOExplanation")]
        public string? IOExplanation { get; set; }
        
        [Postgrest.Attributes.Column("IOSpot")]
        public string? IOSpot { get; set; }
        
        [Postgrest.Attributes.Column("UnitName")]
        public string? UnitName { get; set; }
        
        [Postgrest.Attributes.Column("System")]
        public string? System { get; set; }
        
        [Postgrest.Attributes.Column("StationNumber")]
        public string? StationNumber { get; set; }
        
        [Postgrest.Attributes.Column("IONameNaked")]
        public string? IONameNaked { get; set; }
        
        [Postgrest.Attributes.Column("LinkDevice")]
        public string? LinkDevice { get; set; }
        
        // Properties from KdxMigrationAPI version
        [Postgrest.Attributes.Column("Name")]
        public string? Name { get; set; }
        
        [Postgrest.Attributes.Column("IOType")]
        public int IOType { get; set; }
        
        [Postgrest.Attributes.Column("IsInverted")]
        public bool IsInverted { get; set; }
        
        [Postgrest.Attributes.Column("IsEnabled")]
        public bool IsEnabled { get; set; } = true;
        
        [Postgrest.Attributes.Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Postgrest.Attributes.Column("UpdatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}