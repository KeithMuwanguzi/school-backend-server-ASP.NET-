using System.ComponentModel.DataAnnotations;

namespace SchoolApiService.DTOs;

public class ClassDto(string className, int year)
{

    [Required] [MaxLength(60)] public string ClassName { get; set; } = className;

    [Required] public int Year { get; set; } = year;

    public int? TeacherId { get; set; }
}