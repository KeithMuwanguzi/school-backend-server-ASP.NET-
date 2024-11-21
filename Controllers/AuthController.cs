using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApiService.DTOs;
using SchoolApiService.Models;
using SchoolApiService.Services;

namespace SchoolApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(SchoolDataContext context, EmailService emailService, JwtTokenService tokenService): ControllerBase
{
    [HttpPost("register/student")]
    public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentDto data)
    {
        var userEmailExists = await context.Users.AnyAsync(u => u.Email == data.Email);
        var userPhoneExists = await context.Users.AnyAsync(u => u.PhoneNumber == data.PhoneNumber);
        if(userEmailExists) return BadRequest(new { status = "error",message = "Email already exists!" });
        if(userPhoneExists) return BadRequest(new { status = "error",message = "Phone number already exists!" });
        var newUser = new User(data.FirstName, data.LastName, data.Email, data.PhoneNumber,BCrypt.Net.BCrypt.HashPassword(data.Password), data.Role, data.Gender, data.Nationality);
        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();
        var newStudent = new Student(newUser.UserId,data.PerformanceSummary,data.AdmissionNumber);
        await context.Students.AddAsync(newStudent);
        await context.SaveChangesAsync();
        
        await emailService.SendEmailAsync(newUser.Email, "Email Verification", newUser.FirstName,$"https://{Request.Host}/api/Auth/verify-email?token={newUser.VerificationToken}" );
        
        return Ok(new {status = "success",message = "Student registered. Please make sure you verify your email!"});
    }

    [HttpPost("register/teacher")]
    public async Task<IActionResult> RegisterTeacher([FromBody] RegisterTeacherDto data)
    {
        var userEmailExists = await context.Users.AnyAsync(u => u.Email == data.Email);
        var userPhoneExists = await context.Users.AnyAsync(u => u.PhoneNumber == data.PhoneNumber);
        if(userEmailExists)  return BadRequest(new { status = "error",message = "Email already exists!" });
        if(userPhoneExists) return BadRequest(new {status = "error",message = "Phone number already exists!" });
        var newUser = new User(data.FirstName, data.LastName, data.Email, data.PhoneNumber,BCrypt.Net.BCrypt.HashPassword(data.Password), data.Role, data.Gender, data.Nationality);
        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();

        var newTeacher = new Teacher(newUser.UserId, data.EmployeeNumber, "N/A");
        await context.Teachers.AddAsync(newTeacher);
        await context.SaveChangesAsync();
        
        await emailService.SendEmailAsync(newUser.Email, "Email Verification", newUser.FirstName,$"https://{Request.Host}/api/Auth/verify-email?token={newUser.VerificationToken}" );
        
        return Ok(new {status = "success",message = "Teacher registered, Please make sure you verify your email!"});
    }
    
    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
        if(user == null) return BadRequest("Error: Invalid token");
        
        user.VerifiedAt = DateTime.UtcNow;
        user.IsVerified = true;
        user.VerificationToken = "";
        await context.SaveChangesAsync();
        
        return Ok(new{status = "success", message = "Email Verified successfully"});
    }

    [HttpPut("set-class/{id:int}")]
    public async Task<IActionResult> SetClass(int id,[FromBody] ClassDto data)
    {
        var classExists = await context.Classes.FirstOrDefaultAsync(c => c.ClassName == data.ClassName); 
        var student = await context.Students.FirstOrDefaultAsync(s => s.UserId == id);  
        if(student == null) return BadRequest(new {status = "error",message = "Student not found!" });
        if (classExists != null)
        {
            student.ClassId = classExists.ClassId;
            await context.SaveChangesAsync();
        }
        var newClass = new Class(data.ClassName, data.Year);
        await context.Classes.AddAsync(newClass);
        await context.SaveChangesAsync();
        student.ClassId = newClass.ClassId;
        await context.SaveChangesAsync();
        return Ok(new {status = "success",message = "Class updated successfully!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto data)
    {
        var userLoggedIn = await context.Users.FirstOrDefaultAsync(u => u.Email == data.Email);
        if(userLoggedIn == null) return BadRequest(new {status = "error",message = "Invalid Email or Password" });
        var checkPassword = BCrypt.Net.BCrypt.Verify(data.Password, userLoggedIn.Password);
        if(!checkPassword) return BadRequest(new {status = "error",message = "Invalid Email or Password" });
        if (!userLoggedIn.IsVerified) return Unauthorized(new { status = "pending",message = "Login failed!, please verify your email to continue" });
        object roleData = userLoggedIn.Role switch
        {
            "Student" => (await context.Students.Where(s => s.UserId == userLoggedIn.UserId)
                .Select(s => new { s.Class!.ClassName, s.PerformanceSummary, s.AdmissionNumber })
                .FirstOrDefaultAsync())!,
            "Teacher" => (await context.Teachers.Where(t => t.UserId == userLoggedIn.UserId)
                .Select(t => new { t.PerformanceRating, t.EmployeeNumber, t.Classes, t.Subjects })
                .FirstOrDefaultAsync())!,
            _ => new { status = "error", message = "Invalid Role" }
        };

        var token = tokenService.GenerateToken(userLoggedIn);

        return Ok(new {status = "success",message = "Logged in successfully", data = new {token, user = userLoggedIn,roleData}});
    }
}