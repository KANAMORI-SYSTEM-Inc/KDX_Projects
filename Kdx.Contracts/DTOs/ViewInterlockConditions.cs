using Postgrest.Attributes;
using Postgrest.Models;

[Table("v_interlock_conditions")]
public class ViewInterlockConditions : BaseModel
{
    [PrimaryKey("Id")]
    [Column("Id")]
    public int Id { get; set; }

    [Column("PlcId")]
    public int PlcId { get; set; }

    [Column("CylinderId")]
    public int CylinderId { get; set; }

    [Column("InterlockId")]
    public int InterlockId { get; set; }

    [Column("InterlockName")]
    public string InterlockName { get; set; } = string.Empty;

    [Column("ConditionNumber")]
    public int ConditionNumber { get; set; }

    [Column("ConditionTypeId")]
    public int ConditionTypeId { get; set; }

    [Column("ConditionTypeName")]
    public string ConditionTypeName { get; set; } = string.Empty;
}

