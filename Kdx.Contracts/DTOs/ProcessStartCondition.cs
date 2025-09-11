using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    /// <summary>
    /// 工程の開始条件を管理する中間テーブル
    /// ProcessテーブルのAutoConditionフィールド（複数値）を正規化したもの
    /// </summary>
    [Postgrest.Attributes.Table("ProcessStartCondition")]
    public class ProcessStartCondition : BaseModel
    {
        /// <summary>
        /// ID (主キー)
        /// </summary>
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }

        [Postgrest.Attributes.Column("CycleId")]
        public int? CycleId { get; set; }

        /// <summary>
        /// 工程ID
        /// </summary>
        [Postgrest.Attributes.Column("ProcessId")]
        public int ProcessId { get; set; }

        /// <summary>
        /// 開始条件となる工程詳細ID
        /// </summary>
        [Postgrest.Attributes.Column("StartProcessDetailId")]
        public int StartProcessDetailId { get; set; }

        /// <summary>
        /// 開始センサー（オプション）
        /// </summary>
        [Postgrest.Attributes.Column("StartSensor")]
        public string? StartSensor { get; set; }
    }
}