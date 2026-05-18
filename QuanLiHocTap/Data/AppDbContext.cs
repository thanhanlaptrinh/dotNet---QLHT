using Microsoft.EntityFrameworkCore;
using QuanLiHocTap.Models;

namespace QuanLiHocTap.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = localhost; " +
                "Integrated Security = True; " +
                "Persist Security Info = False; " +
                "Pooling = False; " +
                "MultipleActiveResultSets = False; " +
                "Encrypt = False; " +
                "TrustServerCertificate = True;");
        }
    }
}
