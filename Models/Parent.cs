using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApiService.Models;

public class Parent(int userId, string occupation)
{
    [Key]
    public int ParentId { get; set; }

    [ForeignKey("User")] public int UserId { get; set; } = userId;

    [Required] [MaxLength(100)] public string Occupation { get; set; } = occupation;
    

    public virtual User? User { get; init; }
    public virtual ICollection<Student>? Students { get; init; }
}