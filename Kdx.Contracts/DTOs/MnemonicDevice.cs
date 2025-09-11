using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("MnemonicDevice")]
    public class MnemonicDevice : BaseModel
    {
        [PrimaryKey("ID")]
        [Column("ID")]
        public long? ID { get; set; }

        [Column("MnemonicId")]
        public int MnemonicId { get; set; } // Process: 1, ProcessDetail:2, Operation:3

        [Column("RecordId")]
        public int RecordId { get; set; }  // NemonicIdに対応するテーブルのレコードID

        [Column("DeviceLabel")]
        public string DeviceLabel { get; set; } = "L"; // L (Mの場合もある）

        [Column("StartNum")]
        public int StartNum { get; set; } // 1000

        [Column("OutCoilCount")]
        public int OutCoilCount { get; set; } // 10

        [Column("PlcId")]
        public int PlcId { get; set; }

        [Column("Comment1")]
        public string? Comment1 { get; set; }

        [Column("Comment2")]
        public string? Comment2 { get; set; }

        // L1000 ~ L1009がレコードに対するアウトコイルになる。
    }
}