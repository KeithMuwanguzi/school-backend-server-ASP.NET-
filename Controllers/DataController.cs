using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SchoolApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController(SchoolDataContext context) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPut("make-admin/{id:int}")]
    public async Task<IActionResult> MakeAdmin(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound(new { status = "error", message = "User not found!" });

        user.Role = "Admin";
        await context.SaveChangesAsync();

        return Ok(new { status = "success", message = "User promoted to Admin successfully!" });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("list-users/{role}")]
    public async Task<IActionResult> ListUsersByRole(string role)
    {
        var users = await context.Users.Where(u => u.Role == role)
            .Select(u => new { u.UserId, u.FirstName, u.LastName, u.Email, u.PhoneNumber })
            .ToListAsync();

        if (!users.Any()) return NotFound(new { status = "error", message = "No users found for the given role!" });

        return Ok(new { status = "success", data = users });
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpGet("students")]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await context.Students
            .Include(s => s.Class)
            .Select(s => new
            {
                s.UserId,
                Name = $"{s.User!.FirstName} {s.User.LastName}",
                s.AdmissionNumber,
                s.PerformanceSummary,
                ClassName = s.Class != null ? s.Class.ClassName : "Not Assigned"
            })
            .ToListAsync();

        return Ok(new { status = "success", data = students });
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpGet("teachers")]
    public async Task<IActionResult> GetAllTeachers()
    {
        var teachers = await context.Teachers
            .Include(t => t.Classes)
            .Select(t => new
            {
                t.UserId,
                t.EmployeeNumber,
                t.PerformanceRating,
                Classes = t.Classes != null ? t.Classes.Select(c => c.ClassName): null
            })
            .ToListAsync();

        return Ok(new { status = "success", data = teachers });
    }

    // [Authorize(Roles = "Admin,Teacher")]
    // [HttpPost("assign-subject/{teacherId:int}")]
    // public async Task<IActionResult> AssignSubject(int teacherId, [FromBody] AssignSubjectDto data)
    // {
    //     var teacher = await _context.Teachers.Include(t => t.Subjects).FirstOrDefaultAsync(t => t.UserId == teacherId);
    //     if (teacher == null) return NotFound(new { status = "error", message = "Teacher not found!" });
    //
    //     var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectName == data.SubjectName);
    //     if (subject == null)
    //     {
    //         subject = new Subject(data.SubjectName);
    //         await _context.Subjects.AddAsync(subject);
    //         await _context.SaveChangesAsync();
    //     }
    //
    //     teacher.Subjects.Add(subject);
    //     await _context.SaveChangesAsync();
    //
    //     return Ok(new { status = "success", message = "Subject assigned successfully!" });
    // }

    [Authorize(Roles = "Admin")]
    [HttpDelete("delete-user/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound(new { status = "error", message = "User not found!" });

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return Ok(new { status = "success", message = "User deleted successfully!" });
    }

    [Authorize]
    [HttpGet("profile/{id:int}")]
    public async Task<IActionResult> GetUserProfile(int id)
    {
        var user = await context.Users
            .Where(u => u.UserId == id)
            .Select(u => new
            {
                u.UserId,
                u.FirstName,
                u.LastName,
                u.Email,
                u.PhoneNumber,
                u.Role,
                u.Gender,
                u.Nationality,
                u.IsVerified
            })
            .FirstOrDefaultAsync();

        if (user == null) return NotFound(new { status = "error", message = "User not found!" });

        return Ok(new { status = "success", data = user });
    }
}
