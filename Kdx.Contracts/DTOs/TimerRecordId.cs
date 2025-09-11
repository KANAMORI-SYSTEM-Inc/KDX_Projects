using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("TimerRecordIds")]
    public class TimerRecordId : BaseModel
    {
        [PrimaryKey("TimerId")]
        [Postgrest.Attributes.Column("TimerId")]
        public int TimerId { get; set; }

        [PrimaryKey("RecordId")]
        [Postgrest.Attributes.Column("RecordId")]
        public int RecordId { get; set; }
    }
}