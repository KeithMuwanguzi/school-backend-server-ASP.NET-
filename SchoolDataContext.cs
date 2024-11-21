using Microsoft.EntityFrameworkCore;
using SchoolApiService.Models;

namespace SchoolApiService;

public class SchoolDataContext(DbContextOptions<SchoolDataContext> options): DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<Student> Students { get; init; }
    public DbSet<Attendance> Attendances { get; init; }
    public DbSet<Teacher> Teachers { get; init; }
    public DbSet<Parent> Parents { get; init; }
    public DbSet<Staff> Staff { get; init; }
    public DbSet<Subject> Subjects { get; init; }
    public DbSet<Class> Classes { get; init; }
}