using System;
using AcademyApp.Api.Contracts;
using AcademyApp.Api.Database.EfCore;
using AcademyApp.Api.Models.Entities;

namespace AcademyApp.Api.Services.EfCore;

public class EfCoreListService : IListService
{
    private readonly DataContext _context;
    private readonly IAuthHelper _authHelper;

    public EfCoreListService(DataContext context, IAuthHelper authHelper)
    {
        _context = context;
        _authHelper = authHelper;
    }

    public async Task AddToDoListAsync(ToDoList toDoList)
    {
        var userId = _authHelper.GetUserId();
        toDoList.UserId = userId;
        
        _context.ToDoLists.Add(toDoList);
        await _context.SaveChangesAsync();
    }

    public Task AddToDoListEntryAsync(ToDoListEntry toDoListEntry)
    {
        throw new NotImplementedException();
    }

    public Task DeleteToDoListAsync(int listId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteToDoListEntryAsync(int entryId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ToDoListEntry>> GetToDoListEntriesByListIdAsync(int listId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ToDoList>> GetToDoListsByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ToDoList>> GetToDoListsByUserIdAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateToDoListEntryAsync(ToDoListEntry toDoListEntry)
    {
        throw new NotImplementedException();
    }
}
