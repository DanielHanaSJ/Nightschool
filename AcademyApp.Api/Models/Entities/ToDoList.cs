using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyApp.Api.Models.Entities;

public class ToDoList
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public EntryStatus Status { get; set; } = null!;
    // public bool IsCompleted => Status.Name == "Completed";
    public int UserId { get; set; } // Foreign key to the User table
    public bool IsDeleted { get; set; } = false; // Soft delete flag
    public DateTime? DeletedAt { get; set; } // Timestamp for when the item was deleted
}
