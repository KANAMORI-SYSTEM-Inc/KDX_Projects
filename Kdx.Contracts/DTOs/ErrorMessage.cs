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
    [Postgrest.Attributes.Table("ErrorMessage")]
    public class ErrorMessage : BaseModel
    {
        [PrimaryKey("MnemonicId")]
        [Postgrest.Attributes.Column("MnemonicId")]
        public int MnemonicId { get; set; }
        [PrimaryKey("AlarmId")]
        [Postgrest.Attributes.Column("AlarmId")]
        public int AlarmId { get; set; }
        [Postgrest.Attributes.Column("BaseMessage")]
        public string? BaseMessage { get; set; }
        [Postgrest.Attributes.Column("BaseAlarm")]
        public string? BaseAlarm { get; set; }
        [Postgrest.Attributes.Column("Category1")]
        public string? Category1 { get; set; }
        [Postgrest.Attributes.Column("Category2")]
        public string? Category2 { get; set; }
        [Postgrest.Attributes.Column("Category3")]
        public string? Category3 { get; set; }

        [Postgrest.Attributes.Column("DefaultCountTime")]
        public int DefaultCountTime { get; set; } = 0;
    }
}