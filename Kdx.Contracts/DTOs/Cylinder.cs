using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("Cylinder")]
    public class Cylinder : BaseModel
    {
        [PrimaryKey("Id")]
        [Column("Id")]
        public int Id { get; set; }
        [Column("PlcId")]
        public int PlcId { get; set; } = 0;  // デフォルト値を0に設定
        [Column("PUCO")]
        public string? PUCO { get; set; }
        [Column("CYNum")]
        public string CYNum { get; set; } = string.Empty;
        [Column("Go")]
        public string? Go { get; set; }
        [Column("Back")]
        public string? Back { get; set; }
        [Column("OilNum")]
        public string? OilNum { get; set; }
        [Column("MachineNameId")]
        public int? MachineNameId { get; set; }
        [Column("DriveSubId")]
        public int? DriveSubId { get; set; }
        [Column("PlaceId")]
        public int? PlaceId { get; set; }
        [Column("CYNameSub")]
        public int? CYNameSub { get; set; }
        [Column("SensorId")]
        public int? SensorId { get; set; }
        [Column("FlowType")]
        public string? FlowType { get; set; }
        [Column("ProcessStartCycle")]
        public int? GoSensorCount { get; set; }
        [Column("BackSensorCount")]
        public int? BackSensorCount { get; set; }
        [Column("RetentionSensorGo")]
        public string? RetentionSensorGo { get; set; }
        [Column("RetentionSensorBack")]
        public string? RetentionSensorBack { get; set; }
        [Column("SortNumber")]
        public int? SortNumber { get; set; }
        [Column("FlowCount")]
        public int? FlowCount { get; set; }
        [Column("ManualNumber")]
        public int ManualNumber { get; set; } = 0;
        [Column("FlowCYGo")]
        public string? FlowCYGo { get; set; } = string.Empty;
        [Column("FlowCYBack")]
        public string? FlowCYBack { get; set; } = string.Empty;
    }
}
