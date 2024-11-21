using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApiService.Models;

public class Attendance(int studentId, bool isAttended)
{
    [Key]
    public int Id { get; init; }

    [Required] [ForeignKey("Student")] public int StudentId { get; init; } = studentId;
    [Required] public DateTime AttendanceDate { get; init; } = DateTime.UtcNow;
    [Required] public bool IsAttended { get; set; } = isAttended;
    
    public virtual Student? Student { get; init; }
}