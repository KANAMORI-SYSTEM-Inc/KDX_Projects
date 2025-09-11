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
    [Postgrest.Attributes.Table("ProcessDetail")]
    public class ProcessDetail : BaseModel
    {
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }
        [Postgrest.Attributes.Column("ProcessId")]
        public int ProcessId { get; set; }
        [Postgrest.Attributes.Column("OperationId")]
        public int? OperationId { get; set; }
        [Postgrest.Attributes.Column("DetailName")]
        public string? DetailName { get; set; }
        [Postgrest.Attributes.Column("StartSensor")]
        public string? StartSensor { get; set; }
        [Postgrest.Attributes.Column("CategoryId")]
        public int? CategoryId { get; set; }
        [Postgrest.Attributes.Column("FinishSensor")]
        public string? FinishSensor { get; set; }
        [Postgrest.Attributes.Column("BlockNumber")]
        public int? BlockNumber { get; set; }
        [Postgrest.Attributes.Column("SkipMode")]
        public string? SkipMode { get; set; }
        [Postgrest.Attributes.Column("CycleId")]
        public int? CycleId { get; set; }
        [Postgrest.Attributes.Column("SortNumber")]
        public int? SortNumber { get; set; }
        [Postgrest.Attributes.Column("Comment")]
        public string? Comment { get; set; }
        [Postgrest.Attributes.Column("ILStart")]
        public string? ILStart { get; set; }
        [Postgrest.Attributes.Column("StartTimerId")]
        public int? StartTimerId { get; set; }
    }
}