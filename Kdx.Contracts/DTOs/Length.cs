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
    [Postgrest.Attributes.Table("Length")]
    public class Length : BaseModel
    {
        [PrimaryKey("ID")]
        [Postgrest.Attributes.Column("ID")]
        public int ID { get; set; }
        [Postgrest.Attributes.Column("PlcId")]
        public int PlcId { get; set; }
        [Postgrest.Attributes.Column("LengthName")]
        public string? LengthName { get; set; }
        [Postgrest.Attributes.Column("Device")]
        public string? Device { get; set; }
    }

}