using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("MnemonicTimerDevice")]
    public class MnemonicTimerDevice : BaseModel
    {
        [PrimaryKey("MnemonicId")]
        [Postgrest.Attributes.Column("MnemonicId")]
        public int MnemonicId { get; set; }             // Process: 1, ProcessDetail:2, Operation:3, CY:4

        [PrimaryKey("RecordId")]
        [Postgrest.Attributes.Column("RecordId")]
        public int RecordId { get; set; }               // MnemonicIdに対応するテーブルのレコードID

        [PrimaryKey("TimerId")]
        [Postgrest.Attributes.Column("TimerId")]
        public int TimerId { get; set; }                // TimerテーブルのID（NULLを許可しない）
        [Postgrest.Attributes.Column("TimerCategoryId")]
        public int? TimerCategoryId { get; set; }       // RecordIdに対応する処理番号
        [Postgrest.Attributes.Column("TimerDeviceT")]
        public string TimerDeviceT { get; set; } = "T0";  // RecordIdに対応する処理番号のデバイス
        [Postgrest.Attributes.Column("TimerDeviceZR")]
        public string TimerDeviceZR { get; set; } = "ZR0";        // 外部タイマのデバイス
        [Postgrest.Attributes.Column("PlcId")]
        public int PlcId { get; set; }                  // PLCのID
        [Postgrest.Attributes.Column("CycleId")]
        public int? CycleId { get; set; }               // サイクルID
        [Postgrest.Attributes.Column("Comment1")]
        public string? Comment1 { get; set; }           // コメント1
        [Postgrest.Attributes.Column("Comment2")]
        public string? Comment2 { get; set; }           // コメント2
        [Postgrest.Attributes.Column("Comment3")]
        public string? Comment3 { get; set; }           // コメント2

    }
}
