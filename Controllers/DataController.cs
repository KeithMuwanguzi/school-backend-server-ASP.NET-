using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApiService.DTOs;
using SchoolApiService.Models;

namespace SchoolApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController(SchoolDataContext context) : ControllerBase
{
    [HttpPost("add-class")]
    public async Task<IActionResult> AddClass([FromBody] ClassDto classDto)
    {
        var classExists = await context.Classes.AnyAsync(u => u.ClassName == classDto.ClassName);
        if(classExists) return BadRequest(new {status = "error",message = "Class already exists"});
        var newClass = new Class(classDto.ClassName, classDto.Year);
        await context.Classes.AddAsync(newClass);
        await context.SaveChangesAsync();
        
        return Ok(new {status = "success",message = "Class added successfully"});
    }

    [HttpGet("classes")]
    public async Task<IActionResult> GetClasses()
    {
        var classes = await context.Classes.ToListAsync();  
        return Ok(new {status = "success",data = classes});
    }
}