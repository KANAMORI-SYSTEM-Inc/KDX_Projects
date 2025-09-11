using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("ProsTimeDefinitions")]
    public class ProsTimeDefinitions : BaseModel
    {
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }
        [Required]
        [Postgrest.Attributes.Column("OperationCategoryId")]
        public int OperationCategoryId { get; set; }
        [Required]
        [Postgrest.Attributes.Column("TotalCount")]
        public int TotalCount { get; set; }
        [Required]
        [Postgrest.Attributes.Column("SortOrder")]
        public int SortOrder { get; set; }
        [Required]
        [Postgrest.Attributes.Column("OperationDefinitionsId")]
        public int OperationDefinitionsId { get; set; } // 外部キーとして OperationDifinitions テーブルの ID を参照
    }
    
}
