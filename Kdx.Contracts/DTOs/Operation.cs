using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("Operation")]
    public class Operation : BaseModel
    {
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }
        
        [Postgrest.Attributes.Column("OperationName")]
        public string? OperationName { get; set; }
        
        [Postgrest.Attributes.Column("CYId")]
        public int? CYId { get; set; }
        
        [Postgrest.Attributes.Column("CategoryId")]
        public int? CategoryId { get; set; }
        
        [Postgrest.Attributes.Column("Stay")]
        public string? Stay { get; set; }
        
        [Postgrest.Attributes.Column("GoBack")]
        public string? GoBack { get; set; }
        
        [Postgrest.Attributes.Column("Start")]
        public string? Start { get; set; }
        
        [Postgrest.Attributes.Column("Finish")]
        public string? Finish { get; set; }
        
        [Postgrest.Attributes.Column("Valve1")]
        public string? Valve1 { get; set; }
        
        [Postgrest.Attributes.Column("S1")]
        public string? S1 { get; set; }
        
        [Postgrest.Attributes.Column("S2")]
        public string? S2 { get; set; }
        
        [Postgrest.Attributes.Column("S3")]
        public string? S3 { get; set; }
        
        [Postgrest.Attributes.Column("S4")]
        public string? S4 { get; set; }
        
        [Postgrest.Attributes.Column("S5")]
        public string? S5 { get; set; }
        
        [Postgrest.Attributes.Column("SS1")]
        public string? SS1 { get; set; }
        
        [Postgrest.Attributes.Column("SS2")]
        public string? SS2 { get; set; }
        
        [Postgrest.Attributes.Column("SS3")]
        public string? SS3 { get; set; }
        
        [Postgrest.Attributes.Column("SS4")]
        public string? SS4 { get; set; }
        
        [Postgrest.Attributes.Column("PIL")]
        public int? PIL { get; set; }
        
        [Postgrest.Attributes.Column("SC")]
        public int? SC { get; set; }
        
        [Postgrest.Attributes.Column("FC")]
        public int? FC { get; set; }
        
        [Postgrest.Attributes.Column("CycleId")]
        public int? CycleId { get; set; }
        
        [Postgrest.Attributes.Column("SortNumber")]
        public int? SortNumber { get; set; }
        
        [Postgrest.Attributes.Column("Con")]
        public string? Con { get; set; }
    }
}