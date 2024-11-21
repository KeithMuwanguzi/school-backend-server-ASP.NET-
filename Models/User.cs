using System.ComponentModel.DataAnnotations;

namespace SchoolApiService.Models;

public class User(string firstName, string lastName, string email, string phoneNumber, string password, string role, string gender, string nationality)
{
    [Key]
    public int UserId { get; init; }

    [Required] [StringLength(50)] public string FirstName { get; set; } = firstName;
    [Required] [StringLength(50)] public string LastName { get; set; } = lastName;
    [Required] [StringLength(100)] public string Email { get; set; } = email;
    [Required] [MaxLength(250)][MinLength(8)] public string Password { get; set; } = password;
    [Required] [StringLength(20)] public string PhoneNumber { get; set; } = phoneNumber;
    [StringLength(20)]
    public string? AltPhone { get; set; }
    [StringLength(100)]
    public string? AltEmail { get; set; }
    [Required] [StringLength(20)] public string Role { get; set; } = role;
    [Required] [MaxLength(10)] public string Gender { get; set; } = gender;
    public DateTime? DateOfBirth { get; set; }
    [StringLength(200)]
    public string? Address { get; set; }
    [StringLength(50)] public string Nationality { get; set; } = nationality;
    [StringLength(50)]
    public string? IdNumber { get; set; }
    [StringLength(50)]
    public string? EmergencyContact { get; set; }
    [StringLength(100)]
    public string? ProfilePicture { get; set; } // Path or URL to profile picture
    public DateTime? LastLogin { get; set; }
    public DateTime AccountCreated { get; set; } = DateTime.UtcNow;
    public DateTime? AccountUpdated { get; set; }
    [MaxLength(250)] public string VerificationToken { get; set; } = Guid.NewGuid().ToString();
    public DateTime? VerifiedAt { get; set; }
    public bool IsVerified { get; set; } = false;
    [StringLength(20)]
    public string UserStatus { get; set; } = "Active";
}
