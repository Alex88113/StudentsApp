using SQLite;
using StudentsApp.Models;
namespace StudentsApp.Services;

public class Database
{
    private SQLiteAsyncConnection? _database;
    private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
    
    public async Task <SQLiteAsyncConnection> GetConnectionAsync(string databasePath)
    {
        if (_database is not null)
            return _database;

        await _lock.WaitAsync();
        try
        {
            if (_database is null)
            {
                _database = new SQLiteAsyncConnection(databasePath);
                await _database.CreateTableAsync<Students>();
                Console.WriteLine("The table Students is created.");

                await _database.CreateTableAsync<Groups>();
                Console.WriteLine("The table Groups is created.");
            }
        }
        finally
        {
            _lock.Release();
        }
        return _database;
    }
}