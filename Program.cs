using System.Text;
using StudentsApp.Models;
using StudentsApp.Services;
using StudentsApp.Repositories;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var dbPath = Path.Combine(Environment.CurrentDirectory, "students.db");

var database = new Database();
var connection = await database.GetConnectionAsync(dbPath);

var studentRepo = new StudentRepository(connection);
var groupRepo = new GroupRepository(connection);

while (true)
{
    Console.WriteLine("\n=== StudentsApp ===");
    Console.WriteLine("--- Students ---");
    Console.WriteLine("1. Show all students");
    Console.WriteLine("2. Find student by Id");
    Console.WriteLine("3. Add student");
    Console.WriteLine("4. Update student");
    Console.WriteLine("5. Delete student");
    Console.WriteLine("--- Groups ---");
    Console.WriteLine("6. Show all groups");
    Console.WriteLine("7. Find group by Id");
    Console.WriteLine("8. Add group");
    Console.WriteLine("9. Update group");
    Console.WriteLine("10. Delete group");
    Console.WriteLine("0. Exit");
    Console.Write("Choice: ");

    if (!int.TryParse(Console.ReadLine(), out var choice))
    {
        Console.WriteLine("Enter a number.");
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
        default: Console.WriteLine("Invalid choice."); break;
    }
}

async Task ShowStudents()
{
    var students = await studentRepo.GetAllStudentsAsync();
    if (students.Count == 0) { Console.WriteLine("No students."); return; }
    foreach (var s in students)
        Console.WriteLine($"ID: {s.Id} | {s.FirstName} {s.LastName} | GroupId: {s.GroupId}");
}

async Task FindStudent()
{
    Console.Write("Student Id: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    { Console.WriteLine("Invalid Id."); return; }

    var student = await studentRepo.GetByIdStudentAsync(id);
    if (student == null)
    {
        Console.WriteLine($"Student with Id={id} not found.");
        return;
    }

    Console.WriteLine($"ID: {student.Id} | {student.FirstName} {student.LastName} | GroupId: {student.GroupId}");
}

async Task AddStudent()
{
    var groups = await groupRepo.GetAllGroupsAsync();
    if (groups.Count == 0) { Console.WriteLine("Create at least one group first."); return; }

    Console.Write("First name: ");
    var first = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(first)) { Console.WriteLine("First name cannot be empty."); return; }

    Console.Write("Last name: ");
    var last = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(last)) { Console.WriteLine("Last name cannot be empty."); return; }

    Console.Write("GroupId: ");
    if (!int.TryParse(Console.ReadLine(), out var gid))
    { Console.WriteLine("Invalid GroupId."); return; }

    var group = await groupRepo.GetByIdGroupAsync(gid);
    if (group == null)
    {
        Console.WriteLine($"Group with Id={gid} does not exist.");
        return;
    }

    await studentRepo.AddStudentAsync(new Students { FirstName = first, LastName = last, GroupId = gid });
    Console.WriteLine("Student added.");
}

async Task UpdateStudent()
{
    Console.Write("Student Id: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    { Console.WriteLine("Invalid Id."); return; }

    var student = await studentRepo.GetByIdStudentAsync(id);
    if (student == null)
    {
        Console.WriteLine($"Student with Id={id} not found.");
        return;
    }

    Console.Write($"New first name ({student.FirstName}): ");
    var first = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(first)) student.FirstName = first;

    Console.Write($"New last name ({student.LastName}): ");
    var last = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(last)) student.LastName = last;

    Console.Write($"New GroupId ({student.GroupId}): ");
    var gidInput = Console.ReadLine();
    if (int.TryParse(gidInput, out var gid))
    {
        var group = await groupRepo.GetByIdGroupAsync(gid);
        if (group == null)
        {
            Console.WriteLine($"Group with Id={gid} does not exist. GroupId unchanged.");
        }
        else
        {
            student.GroupId = gid;
        }
    }

    await studentRepo.UpdateStudentAsync(student);
    Console.WriteLine("Student updated.");
}

async Task DeleteStudent()
{
    Console.Write("Student Id: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    { Console.WriteLine("Invalid Id."); return; }

    var student = await studentRepo.GetByIdStudentAsync(id);
    if (student == null)
    {
        Console.WriteLine($"Student with Id={id} not found.");
        return;
    }

    Console.Write($"Delete {student.FirstName} {student.LastName}? (y/n): ");
    if (Console.ReadLine()?.ToLower() != "y") { Console.WriteLine("Cancelled."); return; }

    await studentRepo.DeleteStudentAsync(student);
    Console.WriteLine("Student deleted.");
}

async Task ShowGroups()
{
    var groups = await groupRepo.GetAllGroupsAsync();
    if (groups.Count == 0) { Console.WriteLine("No groups."); return; }
    foreach (var g in groups)
        Console.WriteLine($"ID: {g.Id} | {g.Name}");
}

async Task FindGroup()
{
    Console.Write("Group Id: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    { Console.WriteLine("Invalid Id."); return; }

    var group = await groupRepo.GetByIdGroupAsync(id);
    if (group == null)
    {
        Console.WriteLine($"Group with Id={id} not found.");
        return;
    }

    Console.WriteLine($"ID: {group.Id} | {group.Name}");
}

async Task AddGroup()
{
    Console.Write("Group name: ");
    var name = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(name)) { Console.WriteLine("Name cannot be empty."); return; }

    await groupRepo.AddGroupAsync(new Groups { Name = name });
    Console.WriteLine("Group added.");
}

async Task UpdateGroup()
{
    Console.Write("Group Id: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    { Console.WriteLine("Invalid Id."); return; }

    var group = await groupRepo.GetByIdGroupAsync(id);
    if (group == null)
    {
        Console.WriteLine($"Group with Id={id} not found.");
        return;
    }

    Console.Write($"New name ({group.Name}): ");
    var name = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(name)) group.Name = name;

    await groupRepo.UpdateGroupAsync(group);
    Console.WriteLine("Group updated.");
}

async Task DeleteGroup()
{
    Console.Write("Group Id: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    { Console.WriteLine("Invalid Id."); return; }

    var group = await groupRepo.GetByIdGroupAsync(id);
    if (group == null)
    {
        Console.WriteLine($"Group with Id={id} not found.");
        return;
    }

    Console.Write($"Delete group {group.Name}? (y/n): ");
    if (Console.ReadLine()?.ToLower() != "y") { Console.WriteLine("Cancelled."); return; }

    await groupRepo.DeleteGroupAsync(group);
    Console.WriteLine("Group deleted.");
}