using SQLite;

namespace StudentsApp.Models;

[Table("Groups")]
public class Groups
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
