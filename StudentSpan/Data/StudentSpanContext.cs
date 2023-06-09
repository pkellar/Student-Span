using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentSpan.Models;

namespace StudentSpan.Data
{
    public class StudentSpanContext : IdentityDbContext<ApplicationUser>//DbContext
    {
        public StudentSpanContext (DbContextOptions<StudentSpanContext> options)
            : base(options)
        {
        }

        public DbSet<Student> StudentModel { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Watch> Watch { get; set; }
    }
}
