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
    [Postgrest.Attributes.Table("ProsTime")]
    public class ProsTime : BaseModel
    {
        [PrimaryKey("PlcId")]
        [Required]
        [Postgrest.Attributes.Column("PlcId")]
        public int PlcId { get; set; }
        [Required]
        [Postgrest.Attributes.Column("MnemonicId")]
        public int MnemonicId { get; set; }

        [PrimaryKey("RecordId")]
        [Required]
        [Postgrest.Attributes.Column("RecordId")]
        public int RecordId { get; set; }

        [PrimaryKey("SortId")]
        [Required]
        [Postgrest.Attributes.Column("SortId")]
        public int SortId { get; set; }
        [Required]
        [Postgrest.Attributes.Column("CurrentDevice")]
        public string CurrentDevice { get; set; } = "ZR0";
        [Required]
        [Postgrest.Attributes.Column("PreviousDevice")]
        public string PreviousDevice { get; set; } = "ZR0";
        [Required]
        [Postgrest.Attributes.Column("CylinderDevice")]
        public string CylinderDevice { get; set; } = "ZR0";
        [Required]
        [Postgrest.Attributes.Column("CategoryId")]
        public int CategoryId { get; set; } // OperationDifinitionsのID
    }
}
