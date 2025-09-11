using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("MemoryCategory")]
    public class MemoryCategory : BaseModel
    {
        [PrimaryKey("ID")]
        [Postgrest.Attributes.Column("ID")]
        public int ID { get; set; }
        [Postgrest.Attributes.Column("CategoryName")]
        public string? CategoryName { get; set; }
        [Postgrest.Attributes.Column("Enum")]
        public string? Enum { get; set; }

    }
}