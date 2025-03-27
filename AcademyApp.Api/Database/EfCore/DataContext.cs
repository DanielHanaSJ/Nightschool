using AcademyApp.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcademyApp.Api.Database.EfCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<ToDoListEntry> ToDoListEntries { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<EntryStatus> EntryStatuses { get; set; } = null!;
    public DbSet<ToDoList> ToDoLists { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntryStatus>().HasData(
            new EntryStatus { Id = 1, Name = "Active"},
            new EntryStatus { Id = 2, Name = "Completed"},
            new EntryStatus { Id = 3, Name = "Canceled"}
        );
        
        base.OnModelCreating(modelBuilder);
    }
}
