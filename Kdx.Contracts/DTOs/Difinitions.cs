using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("Difinisions")]
    public class Difinitions : BaseModel
    {
        [PrimaryKey("ID")]
        [Column("ID")]
        public int ID { get; set; }

        [Required]
        [Column("DefName")]
        public string DefName { get; set; } = string.Empty;

        [Required]
        [Column("OutCoilNumber")]
        public int OutCoilNumber { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("Comment1")]
        public string? Comment1 { get; set; }

        [Column("Comment2")]
        public string? Comment2 { get; set; }

        [Column("Label")]
        public string Label { get; set; } = "M";

        [Column("Category")]
        public string Category { get; set; } = "Nan";

        [Column("MnemonicId")]
        public int MnemonicId { get; set; }

        [Column("MemoryCategoryId")]
        public int MemoryCategoryId { get; set; }

    }
}
