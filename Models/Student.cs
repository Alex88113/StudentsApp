using SQLite;

namespace StudentsApp.Models;

[Table("Students")]
public class Students
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    [Indexed]
    [Column("group_id")]
    public int GroupId { get; set; }
}
