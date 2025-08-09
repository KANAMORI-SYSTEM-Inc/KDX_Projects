using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kdx.Contracts.DTOs
{
    /// <summary>
    /// 工程の終了条件を管理する中間テーブル
    /// ProcessテーブルのFinishIdフィールド（複数値）を正規化したもの
    /// </summary>
    [Table("ProcessFinishCondition")]
    public class ProcessFinishCondition
    {
        /// <summary>
        /// ID (主キー)
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 工程ID
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// 終了条件となる工程詳細ID
        /// </summary>
        public int FinishProcessDetailId { get; set; }

        /// <summary>
        /// 終了センサー（オプション）
        /// </summary>
        public string? FinishSensor { get; set; }
    }
}