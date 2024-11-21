using System.ComponentModel.DataAnnotations;

namespace SchoolApiService.DTOs;

public class RegisterTeacherDto(string firstName, string lastName, string email, string phoneNumber, string password, string role, string gender, string nationality, string performanceRating)
{
    [Required] [StringLength(50)] public string FirstName { get; set; } = firstName;
    [Required] [StringLength(50)] public string LastName { get; set; } = lastName;
    [Required] [StringLength(100)] public string Email { get; set; } = email;
    [MinLength(8)] public string Password { get; set; } = password;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string Role { get; set; } = role;
    public string Gender { get; set; } = gender;
    public string Nationality { get; set; } = nationality;
    public string EmployeeNumber { get; set; } = Guid.NewGuid().ToString();
    public string PerformanceRating { get; set; } = performanceRating;
}