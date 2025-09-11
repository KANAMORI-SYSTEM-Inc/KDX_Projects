using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("Cylinder")]
    public class Cylinder : BaseModel
    {
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }
        [Postgrest.Attributes.Column("PlcId")]
        public int PlcId { get; set; } = 0;  // デフォルト値を0に設定
        [Postgrest.Attributes.Column("PUCO")]
        public string? PUCO { get; set; }
        [Postgrest.Attributes.Column("CYNum")]
        public string CYNum { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("Go")]
        public string? Go { get; set; }
        [Postgrest.Attributes.Column("Back")]
        public string? Back { get; set; }
        [Postgrest.Attributes.Column("OilNum")]
        public string? OilNum { get; set; }
        [Postgrest.Attributes.Column("MachineId")]
        public int? MachineId { get; set; }
        [Postgrest.Attributes.Column("DriveSub")]
        public int? DriveSub { get; set; }
        [Postgrest.Attributes.Column("PlaceId")]
        public int? PlaceId { get; set; }
        [Postgrest.Attributes.Column("CYNameSub")]
        public int? CYNameSub { get; set; }
        [Postgrest.Attributes.Column("SensorId")]
        public int? SensorId { get; set; }
        [Postgrest.Attributes.Column("FlowType")]
        public string? FlowType { get; set; }
        [Postgrest.Attributes.Column("ProcessStartCycle")]
        public string? ProcessStartCycle { get; set; }
        [Postgrest.Attributes.Column("GoSensorCount")]
        public int? GoSensorCount { get; set; }
        [Postgrest.Attributes.Column("BackSensorCount")]
        public int? BackSensorCount { get; set; }
        [Postgrest.Attributes.Column("RetentionSensorGo")]
        public string? RetentionSensorGo { get; set; }
        [Postgrest.Attributes.Column("RetentionSensorBack")]
        public string? RetentionSensorBack { get; set; }
        [Postgrest.Attributes.Column("SortNumber")]
        public int? SortNumber { get; set; }
        [Postgrest.Attributes.Column("CycleId")]
        public int? CycleId { get; set; }
        [Postgrest.Attributes.Column("FlowCount")]
        public int? FlowCount { get; set; }
        [Postgrest.Attributes.Column("ManualButton")]
        public string ManualButton { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("FlowCYGo")]
        public string? FlowCYGo { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("FlowCYBack")]
        public string? FlowCYBack { get; set; } = string.Empty;
    }
}
