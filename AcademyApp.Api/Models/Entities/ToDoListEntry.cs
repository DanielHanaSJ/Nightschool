using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyApp.Api.Models.Entities;

public class ToDoListEntry
{
    public int Id { get; set; }
    public int ToDoListId { get; set; } // Foreign key to the ToDoList table
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public EntryStatus Status { get; set; }
    public bool IsCompleted => Status.Name == "Completed";
    public bool IsDeleted { get; set; } = false; // Soft delete flag
    public DateTime? DeletedAt { get; set; } // Timestamp for when the item was deleted
}
