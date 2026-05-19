using Microsoft.Data.SqlClient;
using QuanLiHocTap.Data;
using QuanLiHocTap.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Effects;

namespace QuanLiHocTap.Repository
{
    public class StudentRepository
    {
        public int Count_Students()
        {
            int count = 0;
            string query = "SELECT COUNT(DISTINCT StudentID) FROM Students";
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
