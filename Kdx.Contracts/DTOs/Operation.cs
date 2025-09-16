using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Table("Operation")]
    public class Operation : BaseModel
    {
        [PrimaryKey("Id")]
        [Column("Id")]
        public int Id { get; set; }

        [Column("OperationName")]
        public string? OperationName { get; set; }

        [Column("CYId")]
        public int? CYId { get; set; }

        [Column("CategoryId")]
        public int? CategoryId { get; set; }

        [Column("Stay")]
        public string? Stay { get; set; }

        [Column("GoBack")]
        public string? GoBack { get; set; }

        [Column("Start")]
        public string? Start { get; set; }

        [Column("Finish")]
        public string? Finish { get; set; }

        [Column("Valve1")]
        public string? Valve1 { get; set; }

        [Column("S1")]
        public string? S1 { get; set; }

        [Column("S2")]
        public string? S2 { get; set; }

        [Column("S3")]
        public string? S3 { get; set; }

        [Column("S4")]
        public string? S4 { get; set; }

        [Column("S5")]
        public string? S5 { get; set; }

        [Column("SS1")]
        public string? SS1 { get; set; }

        [Column("SS2")]
        public string? SS2 { get; set; }

        [Column("SS3")]
        public string? SS3 { get; set; }

        [Column("SS4")]
        public string? SS4 { get; set; }

        [Column("PIL")]
        public int? PIL { get; set; }

        [Column("SC")]
        public int? SC { get; set; }

        [Column("FC")]
        public int? FC { get; set; }

        [Column("CycleId")]
        public int? CycleId { get; set; }

        [Column("SortNumber")]
        public int? SortNumber { get; set; }

        [Column("Con")]
        public string? Con { get; set; }
    }
}
