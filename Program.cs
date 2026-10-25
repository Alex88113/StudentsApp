using System.Text;
using StudentsApp.Models;
using StudentsApp.Services;
using StudentsApp.Repositories;

Console.OutputEncoding = Encoding.UTF8;

var dbPath = Path.Combine(Environment.CurrentDirectory, "students.db");

var database = new Database();
var connection = await database.GetConnectionAsync(dbPath);

var studentRepo = new StudentRepository(connection);
var groupRepo = new GroupRepository(connection);

while (true)
{
    Console.WriteLine("\n=== StudentsApp ===");
    Console.WriteLine("--- Студенты ---");
    Console.WriteLine("1. Показать всех студентов");
    Console.WriteLine("2. Найти студента по Id");
    Console.WriteLine("3. Добавить студента");
    Console.WriteLine("4. Обновить студента");
    Console.WriteLine("5. Удалить студента");
    Console.WriteLine("--- Группы ---");
    Console.WriteLine("6. Показать все группы");
    Console.WriteLine("7. Найти группу по Id");
    Console.WriteLine("8. Добавить группу");
    Console.WriteLine("9. Обновить группу");
    Console.WriteLine("10. Удалить группу");
    Console.WriteLine("0. Выход");
    Console.Write("Выбор: ");

    if (!int.TryParse(Console.ReadLine(), out var choice))
    {
        Console.WriteLine("Введите число.");
        continue;
    }

    switch (choice)
    {
        case 1: await ShowStudents(); break;
        case 2: await FindStudent(); break;
        case 3: await AddStudent(); break;
        case 4: await UpdateStudent(); break;
        case 5: await DeleteStudent(); break;
        case 6: await ShowGroups(); break;
        case 7: await FindGroup(); break;
        case 8: await AddGroup(); break;
        case 9: await UpdateGroup(); break;
        case 10: await DeleteGroup(); break;
        case 0: return;
        default: Console.WriteLine("Неверный выбор."); break;
    }
}

async Task ShowStudents()
{
    var students = await studentRepo.GetAllStudentsAsync();
    if (students.Count == 0) { Console.WriteLine("Студентов нет."); return; }
    foreach (var s in students)
        Console.WriteLine($"ID: {s.Id} | {s.FirstName} {s.LastName} | GroupId: {s.GroupId}");
}

async Task FindStudent()
{
    Console.Write("Id студента: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    { Console.WriteLine("Неверный Id."); return; }

    var student = await studentRepo.GetByIdStudentAsync(id);
    if (student == null)
    {
        Console.WriteLine($"Студент с Id={id} не найден.");
        return;
    }

    Console.WriteLine($"ID: {student.Id} | {student.FirstName} {student.LastName} | GroupId: {student.GroupId}");
}

async Task AddStudent()
{
    var groups = await groupRepo.GetAllGroupsAsync();
    if (groups.Count == 0) { Console.WriteLine("Сначала создайте хотя бы одну группу."); return; }

    Console.Write("Имя: ");
    var first = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(first)) { Console.WriteLine("Имя не может быть пустым."); return; }

    Console.Write("Фамилия: ");
    var last = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(last)) { Console.WriteLine("Фамилия не может быть пустой."); return; }

    Console.Write("GroupId: ");
    if (!int.TryParse(Console.ReadLine(), out var gid))
    { Console.WriteLine("Неверный GroupId."); return; }

    var group = await groupRepo.GetByIdGroupAsync(gid);
    if (group == null)
    {
        Console.WriteLine($"Группы с Id={gid} не существует.");
        return;
    }

    await studentRepo.AddStudentAsync(new Students { FirstName = first, LastName = last, GroupId = gid });
    Console.WriteLine("Студент добавлен.");
}

async Task UpdateStudent()
{
    Console.Write("Id студента: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    { Console.WriteLine("Неверный Id."); return; }

    var student = await studentRepo.GetByIdStudentAsync(id);
    if (student == null)
    {
        Console.WriteLine($"Студент с Id={id} не найден.");
        return;
    }

    Console.Write($"Новое имя ({student.FirstName}): ");
    var first = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(first)) student.FirstName = first;

    Console.Write($"Новая фамилия ({student.LastName}): ");
    var last = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(last)) student.LastName = last;

    Console.Write($"Новый GroupId ({student.GroupId}): ");
    var gidInput = Console.ReadLine();
    if (int.TryParse(gidInput, out var gid))
    {
        var group = await groupRepo.GetByIdGroupAsync(gid);
        if (group == null)
        {
            Console.WriteLine($"Группы с Id={gid} не существует. GroupId не изменён.");
        }
        else
        {
            student.GroupId = gid;
        }
    }

    await studentRepo.UpdateStudentAsync(student);
    Console.WriteLine("Студент обновлён.");
}

async Task DeleteStudent()
{
    Console.Write("Id студента: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    { Console.WriteLine("Неверный Id."); return; }

    var student = await studentRepo.GetByIdStudentAsync(id);
    if (student == null)
    {
        Console.WriteLine($"Студент с Id={id} не найден.");
        return;
    }

    Console.Write($"Удалить {student.FirstName} {student.LastName}? (y/n): ");
    if (Console.ReadLine()?.ToLower() != "y") { Console.WriteLine("Отменено."); return; }

    await studentRepo.DeleteStudentAsync(student);
    Console.WriteLine("Студент удалён.");
}

async Task ShowGroups()
{
    var groups = await groupRepo.GetAllGroupsAsync();
    if (groups.Count == 0) { Console.WriteLine("Групп нет."); return; }
    foreach (var g in groups)
        Console.WriteLine($"ID: {g.Id} | {g.Name}");
}

async Task FindGroup()
{
    Console.Write("Id группы: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    { Console.WriteLine("Неверный Id."); return; }

    var group = await groupRepo.GetByIdGroupAsync(id);
    if (group == null)
    {
        Console.WriteLine($"Группа с Id={id} не найдена.");
        return;
    }

    Console.WriteLine($"ID: {group.Id} | {group.Name}");
}

async Task AddGroup()
{
    Console.Write("Название группы: ");
    var name = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Название не может быть пустым."); return; }

    await groupRepo.AddGroupAsync(new Groups { Name = name });
    Console.WriteLine("Группа добавлена.");
}

async Task UpdateGroup()
{
    Console.Write("Id группы: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    { Console.WriteLine("Неверный Id."); return; }

    var group = await groupRepo.GetByIdGroupAsync(id);
    if (group == null)
    {
        Console.WriteLine($"Группа с Id={id} не найдена.");
        return;
    }

    Console.Write($"Новое название ({group.Name}): ");
    var name = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(name)) group.Name = name;

    await groupRepo.UpdateGroupAsync(group);
    Console.WriteLine("Группа обновлена.");
}

async Task DeleteGroup()
{
    Console.Write("Id группы: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    { Console.WriteLine("Неверный Id."); return; }

    var group = await groupRepo.GetByIdGroupAsync(id);
    if (group == null)
    {
        Console.WriteLine($"Группа с Id={id} не найдена.");
        return;
    }

    Console.Write($"Удалить группу {group.Name}? (y/n): ");
    if (Console.ReadLine()?.ToLower() != "y") { Console.WriteLine("Отменено."); return; }

    await groupRepo.DeleteGroupAsync(group);
    Console.WriteLine("Группа удалена.");
}