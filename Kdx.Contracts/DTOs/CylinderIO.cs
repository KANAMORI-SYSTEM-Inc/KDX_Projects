using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    /// <summary>
    /// CYテーブルとIOテーブルを関連付ける中間テーブル
    /// </summary>
    [Postgrest.Attributes.Table("CylinderIO")]
    public class CylinderIO : BaseModel
    {
        [PrimaryKey("CylinderId")]
        [Postgrest.Attributes.Column("CylinderId")]
        public int CylinderId { get; set; }  // CYテーブルのId
        
        [PrimaryKey("IOAddress")]
        [Postgrest.Attributes.Column("IOAddress")]
        public string IOAddress { get; set; } = string.Empty;  // IOテーブルのAddress
        
        // Alias for compatibility
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string Address 
        { 
            get => IOAddress; 
            set => IOAddress = value; 
        }
        
        [PrimaryKey("PlcId")]
        [Postgrest.Attributes.Column("PlcId")]
        public int PlcId { get; set; } = 0;  // IOテーブルのPlcId（複合キーの一部）
        
        /// <summary>
        /// IOの用途タイプ
        /// </summary>
        [Postgrest.Attributes.Column("IOType")]
        public string IOType { get; set; } = string.Empty;  // "GoSensor", "BackSensor", "GoValve", "BackValve" など
        
        /// <summary>
        /// 表示順序
        /// </summary>
        [Postgrest.Attributes.Column("SortOrder")]
        public int? SortOrder { get; set; }
        
        /// <summary>
        /// 備考
        /// </summary>
        [Postgrest.Attributes.Column("Comment")]
        public string? Comment { get; set; }
    }
}