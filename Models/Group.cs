using SQLite;

namespace StudentsApp.Models;

[Table("Group")]
public class Group
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}