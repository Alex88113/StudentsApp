using SQLite;

namespace StudentsApp.Models;

[Table("Student")]
public class Student
{
    [PrimaryKey, AutoIncrement, Column("id")]
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    [Indexed]
    [Column("group_id")]
    public int GroupId { get; set; }
}