using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SchoolApiService.Models;

public class Student(int userId,string performanceSummary, string admissionNumber)
{
    [Key]
    public int StudentId { get; set; }

    [ForeignKey("User")] public int UserId { get; set; } = userId;

    [Required] [MaxLength(200)] public string AdmissionNumber { get; set; } = admissionNumber;

    [ForeignKey("Class")] public int? ClassId { get; set; }

    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

    [ForeignKey("Parent")] public int? ParentId { get; set; }

    [MaxLength(500)] public string PerformanceSummary { get; set; } = performanceSummary;

    public virtual User? User { get; init; }
    public virtual Class? Class { get; set; }
    public virtual Parent? Parent { get; init; }

}