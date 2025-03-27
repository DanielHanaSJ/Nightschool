using System;
using System.Data;
using AcademyApp.Api.Contracts;
using AcademyApp.Api.Database.Dapper;
using AcademyApp.Api.Models.Entities;
using Dapper;

namespace AcademyApp.Api.Services.Dapper;

public class DapperListService : IListService
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly IAuthHelper _authHelper;

    public DapperListService(IDbConnectionFactory dbConnectionFactory, IAuthHelper authHelper)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _authHelper = authHelper;
    }

    public async Task<IEnumerable<ToDoList>> GetToDoListsByUserIdAsync()
    {
        var userId = _authHelper.GetUserId();
        
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var sql = "SELECT * FROM ToDoLists l WHERE UserId = @UserId WHERE IsDeleted = 0";

        return await connection.QueryAsync<ToDoList>(sql, new { UserId = userId });
    }

    public async Task<IEnumerable<ToDoListEntry>> GetToDoListEntriesByListIdAsync(int listId)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var sql = "SELECT * FROM ToDoListEntries WHERE ToDoListId = @ListId";

        return await connection.QueryAsync<ToDoListEntry>(sql, new { ListId = listId });
    }

    public async Task AddToDoListAsync(ToDoList toDoList)
    {
        var userId = _authHelper.GetUserId();

        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var sql = "INSERT INTO ToDoLists (Title, Description, StatusId, UserId) VALUES (@Title, @Description, @StatusId, @UserId)";

        await connection.ExecuteAsync(sql, new
        {
            toDoList.Title,
            toDoList.Description,
            CreatedAt = DateTime.UtcNow,
            StatusId = toDoList.Status.Id,
            userId
        });
    }

    public async Task AddToDoListEntryAsync(ToDoListEntry toDoListEntry)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var sql = "INSERT INTO ToDoListEntries (ToDoListId, Title, Description, Status) VALUES (@ToDoListId, @Title, @Description, @Status)";

        await connection.ExecuteAsync(sql, new
        {
            toDoListEntry.ToDoListId,
            toDoListEntry.Title,
            toDoListEntry.Description,
            Status = toDoListEntry.Status,
        });
    }

    public async Task UpdateToDoListEntryAsync(ToDoListEntry toDoListEntry)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var sql = "UPDATE ToDoListEntries SET Title = @Title, Description = @Description, Status = @Status WHERE Id = @Id";

        await connection.ExecuteAsync(sql, new
        {
            toDoListEntry.Title,
            toDoListEntry.Description,
            Status = toDoListEntry.Status,
            toDoListEntry.Id
        });
    }

    public async Task DeleteToDoListEntryAsync(int entryId)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var sql = "DELETE FROM ToDoListEntries WHERE Id = @Id";

        await connection.ExecuteAsync(sql, new { Id = entryId });
    }

    public async Task DeleteToDoListAsync(int listId)
    {
        using var db = await _dbConnectionFactory.CreateConnectionAsync();

        await db.ExecuteAsync(
            "usp_DeleteToDoList",
            new
            {
                listId = listId
            },
            commandType: CommandType.StoredProcedure);
    }
}
