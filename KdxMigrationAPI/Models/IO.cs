using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KdxMigrationAPI.Models
{
    /// <summary>
    /// IOテーブル - 入出力デバイス情報
    /// </summary>
    [Table("IO")]
    public class IO
    {
        /// <summary>
        /// アドレス（複合キーの一部）
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// PLC ID（複合キーの一部）
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public int PlcId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(255)]
        public string? Name { get; set; }

        /// <summary>
        /// 入出力タイプ (0:入力, 1:出力)
        /// </summary>
        public int IOType { get; set; }

        /// <summary>
        /// 反転フラグ
        /// </summary>
        public bool IsInverted { get; set; }

        /// <summary>
        /// 有効フラグ
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}