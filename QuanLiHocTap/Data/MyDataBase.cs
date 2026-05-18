using Microsoft.EntityFrameworkCore;
using QuanLiHocTap.Models;

namespace QuanLiHocTap.Data
{
    public class MyDataBase : DbContext
    {
        // Khởi tạo và đọc chuỗi Connection String tên là 'AcademicManagementConn' từ App.config
        public MyDataBase() : base("name=AcademicManagementConn")
        {
            // Ngăn EF tự ý khởi tạo lại cấu trúc bảng vì ta quản lý bằng file script SQL độc lập
            Database.SetInitializer<MyDataBase>(null);
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Offering> Offerings { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }
    }
}
