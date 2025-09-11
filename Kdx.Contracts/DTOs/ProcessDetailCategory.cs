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
    [Postgrest.Attributes.Table("ProcessDetailCategory")]
    public class ProcessDetailCategory : BaseModel
    {
        [PrimaryKey("ID")]
        [Postgrest.Attributes.Column("ID")]
        public int Id { get; set; }
        [Postgrest.Attributes.Column("CategoryName")]
        public string? CategoryName { get; set; }
        [Postgrest.Attributes.Column("Description")]
        public string? Description { get; set; }
        [Postgrest.Attributes.Column("ShortName")]
        public string? ShortName { get; set; }


    }

}