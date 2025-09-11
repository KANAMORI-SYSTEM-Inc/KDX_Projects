using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("Place")]
    public class Place : BaseModel
    {
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }
        [Postgrest.Attributes.Column("ModelId")]
        public int? ModelId { get; set; }
        [Postgrest.Attributes.Column("PlaceName")]
        public string? PlaceName { get; set; }
    }
}