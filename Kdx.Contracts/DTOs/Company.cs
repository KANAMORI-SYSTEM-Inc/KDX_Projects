using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("Company")]
    public class Company : BaseModel
    {
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }
        
        [Postgrest.Attributes.Column("CompanyName")]
        public string? CompanyName { get; set; }
        
        [Postgrest.Attributes.Column("CreatedAt")]
        public string? CreatedAt { get; set; }
    }
}
