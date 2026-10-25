using StudentsApp.Models;
using SQLite;

namespace StudentsApp.Repositories;

public class GroupRepository
{
    private readonly SQLiteAsyncConnection _connection;

    public GroupRepository(SQLiteAsyncConnection connection)
    {
        _connection = connection;
    }

    public async Task<Groups> AddGroupAsync(Groups group)
    {
        await _connection.InsertAsync(group);
        return group;
    }

    public async Task<List<Groups>> GetAllGroupsAsync()
    {
        return await _connection.Table<Groups>().ToListAsync();
    }

    public async Task<Groups?> GetByIdGroupAsync(int groupId)
    {
        return await _connection.FindAsync<Groups>(groupId);
    }

    public async Task<Groups> UpdateGroupAsync(Groups group)
    {
        await _connection.UpdateAsync(group);
        return group;
    }

    public async Task<int> DeleteGroupAsync(Groups group)
    {
        return await _connection.DeleteAsync(group);
    }
}
