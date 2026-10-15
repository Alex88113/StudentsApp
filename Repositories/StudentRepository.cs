using System.Collections.Generic;
using System.Threading.Tasks;

using StudentsApp.Models;
using SQLite;

namespace StudentsApp.Repositories;

public class StudentRepository
{
    private readonly SQLiteAsyncConnection _connection;
    public StudentRepository(SQLiteAsyncConnection connection) {
        _connection = connection;
    }

    public async Task<Students> AddStudentAsync(Students student)
    {
       await _connection.InsertAsync(student);
       return student; 
    }

    public async Task<Students> UpdateStudentAsync(Students student)
    {
        await _connection.UpdateAsync(student);
        return student;
    }

    public async Task<int> DeleteStudentAsync(Students student)
    {
        return await _connection.DeleteAsync(student);
    }

    public async Task<Students?> GetByIdStudentAsync(int studentId) {
        return await _connection.GetAsync<Students>(studentId);
    }

    public async Task<List<Students>> GetAllStudentsAsync()
    {
        return await _connection.Table<Students>().ToListAsync();
    }
}