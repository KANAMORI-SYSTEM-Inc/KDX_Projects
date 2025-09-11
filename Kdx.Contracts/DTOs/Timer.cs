using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("Timer")]
    public class Timer : BaseModel
    {
        [PrimaryKey("ID")]
        [Postgrest.Attributes.Column("ID")]
        public int ID { get; set; }
        
        [Postgrest.Attributes.Column("CycleId")]
        public int? CycleId { get; set; }
        
        [Postgrest.Attributes.Column("TimerCategoryId")]
        public int? TimerCategoryId { get; set; }
        
        [Postgrest.Attributes.Column("TimerNum")]
        public int? TimerNum { get; set; }
        
        [Postgrest.Attributes.Column("TimerName")]
        public string? TimerName { get; set; }
        
        [Postgrest.Attributes.Column("MnemonicId")]
        public int? MnemonicId { get; set; }
        
        [Postgrest.Attributes.Column("Example")]
        public int? Example { get; set; }
    }
}