using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Postgrest.Attributes;
using Postgrest.Models;

namespace Kdx.Contracts.DTOs
{
    [Postgrest.Attributes.Table("IOHistory")]
    public class IOHistory : BaseModel
    {
        [PrimaryKey("Id")]
        [Postgrest.Attributes.Column("Id")]
        public int Id { get; set; }
        [Postgrest.Attributes.Column("IoAddress")]
        public string? IoAddress { get; set; } // 変更されたIOのアドレス
        [Postgrest.Attributes.Column("IoPlcId")]
        public int? IoPlcId { get; set; } // 変更されたIOのPLC ID
        [Postgrest.Attributes.Column("PropertyName")]
        public string? PropertyName { get; set; } // 変更されたプロパティ名
        [Postgrest.Attributes.Column("OldValue")]
        public string? OldValue { get; set; } // 変更前の値
        [Postgrest.Attributes.Column("NewValue")]
        public string? NewValue { get; set; } // 変更後の値
        [Postgrest.Attributes.Column("ChangedAt")]
        public string? ChangedAt { get; set; } // 変更後の値

        [Postgrest.Attributes.Column("ChangedBy")]
        public string? ChangedBy { get; set; } // 変更者（今回は固定値でもOK）
    }
}