using SQLite;
using StudentsApp.Models;
using StudentsApp.Services;
using StudentsApp.Repositories;

var databasePath = Path.Combine(Directory.GetCurrentDirectory(), "academic_db.db");

var db = new Database(databasePath);
var conn = await db.GetConnectionAsync();

Console.WriteLine("Инициализация БД");
Console.WriteLine($"Путь к базе данных: {databasePath}");
Console.WriteLine("========================================\n");

Console.WriteLine("Добавление студентов.....");
var repoStudent = new StudentRepository(conn);
var student1 = new Students
{
    FirstName = "Дмитрий",
    LastName = "Мартынов",
    GroupId = 1
};
var student2 = new Students
{
    FirstName = "Дмитрий",
    LastName = "Берестов",
    GroupId = 2
};
var student3 = new Students
{
    FirstName = "Егор",
    LastName = "Шеметов",
    GroupId = 3
};

await repoStudent.AddStudentAsync(student1);
await repoStudent.AddStudentAsync(student2);
await repoStudent.AddStudentAsync(student3);
Console.WriteLine("Студенты добавлены");

Console.WriteLine("================= Все студенты в базе =================");
var allStudents = await repoStudent.GetAllStudentsAsync();
Console.WriteLine(allStudents);

foreach (var student in allStudents)
{
    Console.WriteLine($"ID: {student.Id} | FirstName: {student.FirstName} | LastName: {student.LastName}");
}
