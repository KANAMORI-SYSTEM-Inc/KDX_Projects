using System.ComponentModel.DataAnnotations;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("Machine")]
    public class Machine : BaseModel
    {
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }
        
        [Postgrest.Attributes.Column("MachineName")]
        public string MachineName { get; set; } = string.Empty;
        
        [Postgrest.Attributes.Column("ShortName")]
        public string ShortName { get; set; } = string.Empty;
    }
}