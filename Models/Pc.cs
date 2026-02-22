using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("pc")]
public class Pc : BaseModel
{
    [PrimaryKey("pc_id")]
    public long PcId { get; set; }

    [Column("user_id")]
    public string? UserId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("cpu")]
    public string? Cpu { get; set; }

    [Column("gpu")]
    public string? Gpu { get; set; }

    [Column("note")]
    public string? Note { get; set; }
}