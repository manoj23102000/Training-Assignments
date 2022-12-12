using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TallyAssignment3.Models;

public partial class Student
{
    public int StudId { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? Class { get; set; }
}
