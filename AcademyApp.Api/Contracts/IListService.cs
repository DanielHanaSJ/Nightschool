using System;
using AcademyApp.Api.Models.Entities;

namespace AcademyApp.Api.Contracts;

public interface IListService
{
    Task<IEnumerable<ToDoList>> GetToDoListsByUserIdAsync();
    Task<IEnumerable<ToDoListEntry>> GetToDoListEntriesByListIdAsync(int listId);
    Task AddToDoListAsync(ToDoList toDoList);
    Task AddToDoListEntryAsync(ToDoListEntry toDoListEntry);
    Task UpdateToDoListEntryAsync(ToDoListEntry toDoListEntry);
    Task DeleteToDoListEntryAsync(int entryId);
    Task DeleteToDoListAsync(int listId);
}
