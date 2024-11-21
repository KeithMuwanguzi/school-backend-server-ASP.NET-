using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApiService.Models;

public class Teacher(int userId, string employeeNumber, string performanceRating)
{
    [Key]
    public int TeacherId { get; set; }

    [ForeignKey("User")] public int UserId { get; set; } = userId;

    [MaxLength(60)] public string EmployeeNumber { get; set; } = employeeNumber;

    [ForeignKey("Subject")] public int? SubjectId { get; set; }

    public DateTime HireDate { get; set; } = DateTime.UtcNow;

    [MaxLength(60)] public string PerformanceRating { get; set; } = performanceRating;

    public virtual User? User { get; init; }
    public virtual ICollection<Class>? Classes { get; set; }
    public virtual ICollection<Subject>? Subjects { get; set; }
}