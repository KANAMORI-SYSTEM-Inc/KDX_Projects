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
    [Postgrest.Attributes.Table("ProcessError")]
    public class ProcessError : BaseModel
    {
        [PrimaryKey("PlcId", false)]
        [Postgrest.Attributes.Column("PlcId")]
        public int? PlcId { get; set; }

        [Postgrest.Attributes.Column("CycleId")]
        public int? CycleId { get; set; }
        
        [Postgrest.Attributes.Column("Device")]
        public string? Device { get; set; }
        
        [Postgrest.Attributes.Column("MnemonicId")]
        public int? MnemonicId { get; set; }
        
        [Postgrest.Attributes.Column("RecordId")]
        public int? RecordId { get; set; }

        [Postgrest.Attributes.Column("AlarmId")]
        public int? AlarmId { get; set; }

        [PrimaryKey("ErrorNum", false)]
        [Postgrest.Attributes.Column("ErrorNum")]
        public int? ErrorNum { get; set; }
        
        [Postgrest.Attributes.Column("Comment1")]
        public string? Comment1 { get; set; }
        
        [Postgrest.Attributes.Column("Comment2")]
        public string? Comment2 { get; set; }
        
        [Postgrest.Attributes.Column("Comment3")]
        public string? Comment3 { get; set; }
        
        [Postgrest.Attributes.Column("Comment4")]
        public string? Comment4 { get; set; }
        
        [Postgrest.Attributes.Column("AlarmComment")]
        public string? AlarmComment { get; set; }
        
        [Postgrest.Attributes.Column("MessageComment")]
        public string? MessageComment { get; set; }
        
        [Postgrest.Attributes.Column("ErrorTime")]
        public int? ErrorTime { get; set; }
        
        [Postgrest.Attributes.Column("ErrorTimeDevice")]
        public string? ErrorTimeDevice { get; set; }
        
        // Note: ErrorCountTime doesn't exist in the database table
        // This was likely a duplicate of ErrorTime in the original Access database

    }
}
