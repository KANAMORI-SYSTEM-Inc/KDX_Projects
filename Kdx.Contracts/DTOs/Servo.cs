using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("Servo")]
    public class Servo : BaseModel
    {
        [PrimaryKey("ID")]
        [Postgrest.Attributes.Column("ID")]
        public int ID { get; set; }
        [Postgrest.Attributes.Column("PlcId")]
        public int PlcId { get; set; }
        [Postgrest.Attributes.Column("CylinderId")]
        public int CylinderId { get; set; }
        [Postgrest.Attributes.Column("Busy")]
        public string Busy { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("PositioningStart")]
        public string PositioningStart { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("Prefix")]
        public string Prefix { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("AxisNumber")]
        public int AxisNumber { get; set; }
        [Postgrest.Attributes.Column("AxisStop")]
        public string AxisStop { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("PositioningStartNum")]
        public string PositioningStartNum { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("GS")]
        public string GS { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("JogSpeed")]
        public string JogSpeed { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("StartFowardJog")]
        public string StartFowardJog { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("StartReverseJog")]
        public string StartReverseJog { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("CommandPosition")]
        public string CommandPosition { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("CurrentValue")]
        public string CurrentValue { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("OriginalPosition")]
        public string OriginalPosition { get; set; } = string.Empty;
        [Postgrest.Attributes.Column("Status")]
        public string Status { get; set; } = string.Empty;


    }
}