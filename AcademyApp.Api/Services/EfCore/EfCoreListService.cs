using System;
using AcademyApp.Api.Contracts;
using AcademyApp.Api.Models.Entities;

namespace AcademyApp.Api.Services.EfCore;

public class EfCoreListService : IListService
{
    public Task AddToDoListAsync(ToDoList toDoList)
    {
        throw new NotImplementedException();
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
