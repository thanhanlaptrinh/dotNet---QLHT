using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Effects;
using QuanLiHocTap.Data;

namespace QuanLiHocTap.Repository
{
    public class StudentRepository
    {
        public int Count_Students()
        {
            using (AppDbContext db = new AppDbContext())
            {
                return db.Students.Count();
            }
        }
    }

}
