using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("MnemonicSpeedDevice")]
    public class MnemonicSpeedDevice : BaseModel
    {
        [PrimaryKey("ID")]
        [Column("ID")]
        public int ID { get; set; }

        [Column("CylinderId")]
        public int CylinderId { get; set; }

        [Column("Device")]
        public string Device { get; set; } = "D0";

        [Column("PlcId")]
        public int PlcId { get; set; }
    }
}