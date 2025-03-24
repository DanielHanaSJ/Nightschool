using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyApp.Api.Models.Entities;

public class EntryStatus
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}
