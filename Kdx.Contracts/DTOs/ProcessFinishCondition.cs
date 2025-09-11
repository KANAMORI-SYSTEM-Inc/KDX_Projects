using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    /// <summary>
    /// 工程の終了条件を管理する中間テーブル
    /// ProcessテーブルのFinishIdフィールド（複数値）を正規化したもの
    /// </summary>
    [Postgrest.Attributes.Table("ProcessFinishCondition")]
    public class ProcessFinishCondition : BaseModel
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
        /// 終了条件となる工程詳細ID
        /// </summary>
        [Postgrest.Attributes.Column("FinishProcessDetailId")]
        public int FinishProcessDetailId { get; set; }

        /// <summary>
        /// 終了センサー（オプション）
        /// </summary>
        [Postgrest.Attributes.Column("FinishSensor")]
        public string? FinishSensor { get; set; }
    }
}