using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApiService.Models;

public class Subject(int teacherId,string name,string description,int classId)
{
    [Key]
    public int SubjectId { get; set; }

    [Required] [MaxLength(100)] public string Name { get; set; } = name;

    [MaxLength(200)] public string Description { get; set; } = description;

    [ForeignKey("Class")] public int ClassId { get; set; } = classId;

    [ForeignKey("Teacher")] public int TeacherId { get; set; } = teacherId;

    public virtual Class? Class { get; set; }
    public virtual Teacher? Teacher { get; set; }
}