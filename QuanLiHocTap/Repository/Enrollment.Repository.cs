using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Effects;
using QuanLiHocTap.Data;

namespace QuanLiHocTap.Repository
{
    public class EnrollmentRepository
    {
        AppDbContext db;
        public EnrollmentRepository() {
            db = new AppDbContext();
        }
        public int Count_Enrollment()
        {
            return db.Enrollments.Count();
        }
    }

}
