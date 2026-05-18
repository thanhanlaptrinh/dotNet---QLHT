using Microsoft.EntityFrameworkCore;
using QuanLiHocTap.Models;

namespace QuanLiHocTap.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Offering> Offerings { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseSqlServer("Data Source=QUOTHANH;Initial Catalog = TechShop; Integrated Security = True; Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;");
            //optionsBuilder.UseSqlServer(
            //    "Server=localhost;" +
            //    "Database=QuanLiHocTap;" +
            //    "Trusted_Connection=True;" +
            //    "TrustServerCertificate=True;");
        }
    }
}