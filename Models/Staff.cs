using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApiService.Models;

public class Staff(int userId, string position)
{
    [Key]
    public int StaffId { get; init; }

    [Required] [ForeignKey("User")] public int UserId { get; init; } = userId;
    [Required] [MaxLength(60)] public string Position { get; set; } = position; 
    
    public virtual User? User { get; init; }
    
}