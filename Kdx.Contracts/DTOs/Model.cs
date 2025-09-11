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
    [Postgrest.Attributes.Table("Model")]
    public class Model : BaseModel
    {
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }
        
        [Postgrest.Attributes.Column("ModelName")]
        public string? ModelName { get; set; }
        
        [Postgrest.Attributes.Column("CompanyId")]
        public int? CompanyId { get; set; }
    }
    
}