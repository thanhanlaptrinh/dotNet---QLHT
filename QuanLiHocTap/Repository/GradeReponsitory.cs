using Microsoft.Data.SqlClient;
using QuanLiHocTap.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLiHocTap.Repository
{
    public class GradeReponsitory
    {
        public int GetPendingGradesCount()
        {
            int count = 0;
            string query = @"SELECT COUNT(DISTINCT EnrollmentID) FROM Grades WHERE GradeStatus = 'Submitted'";
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        var result = cmd.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            count = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi đếm số lớp chờ duyệt: " + ex.Message);
                    }
                }
            }
            return count;
        }
    }
}
