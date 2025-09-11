using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    /// <summary>
    /// OperationとIOの関連付けを管理する中間テーブル
    /// </summary>
    [Postgrest.Attributes.Table("OperationIO")]
    public class OperationIO : BaseModel
    {
        [PrimaryKey("OperationId")]
        [Postgrest.Attributes.Column("OperationId")]
        public int OperationId { get; set; }

        [PrimaryKey("IOAddress")]
        [Postgrest.Attributes.Column("IOAddress")]
        public string IOAddress { get; set; } = string.Empty;
        
        // Alias for compatibility
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string Address 
        { 
            get => IOAddress; 
            set => IOAddress = value; 
        }

        [PrimaryKey("PlcId")]
        [Postgrest.Attributes.Column("PlcId")]
        public int PlcId { get; set; } = 0;  // デフォルト値を0に設定

        /// <summary>
        /// IOの用途（Input/Output/Condition/Interlockなど）
        /// </summary>
        [Postgrest.Attributes.Column("IOUsage")]
        public string IOUsage { get; set; } = string.Empty;
        
        // Alias for compatibility
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string Usage 
        { 
            get => IOUsage; 
            set => IOUsage = value; 
        }

        /// <summary>
        /// 表示順序
        /// </summary>
        [Postgrest.Attributes.Column("SortOrder")]
        public int? SortOrder { get; set; }

        /// <summary>
        /// コメント
        /// </summary>
        [Postgrest.Attributes.Column("Comment")]
        public string? Comment { get; set; }
    }
}