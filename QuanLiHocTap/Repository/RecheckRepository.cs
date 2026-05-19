using Microsoft.Data.SqlClient;
using QuanLiHocTap.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QuanLiHocTap.Repository
{
    public class RecheckRepository
    {
        public int countRechecks() {
            int count = 0;
            string query = "SELECT COUNT(DISTINCT GradeID) FROM Grades WHERE RecheckNote IS NOT NULL AND GradeStatus = 'Submitted'";
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return count;
                }
            }
        }
    }
}
