using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApiService.Models;

public class Class(string className, int year)
{
    [Key]
    public int ClassId { get; init; }

    [Required] [MaxLength(60)] public string ClassName { get; set; } = className;

    [Required] public int Year { get; set; } = year;

    [ForeignKey("Teacher")] public int? TeacherId { get; set; }

    public virtual Teacher? Teacher { get; set; }
    public virtual ICollection<Student>? Students { get; set; }
    public virtual ICollection<Subject>? Subjects { get; set; }
}