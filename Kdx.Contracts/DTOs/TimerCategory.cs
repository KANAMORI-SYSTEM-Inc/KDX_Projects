using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("TimerCategory")]
    public class TimerCategory : BaseModel
    {
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }
        [Postgrest.Attributes.Column("CategoryName")]
        public string? CategoryName { get; set; }
        [Postgrest.Attributes.Column("CategoryFig")]
        public string? CategoryFig { get; set; }
    }
}