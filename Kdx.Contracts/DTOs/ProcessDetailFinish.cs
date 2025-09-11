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
        
        // CycleIdはProcessDetailFinishテーブルには存在しない
        // ProcessDetailテーブル経由で取得する必要がある
        
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