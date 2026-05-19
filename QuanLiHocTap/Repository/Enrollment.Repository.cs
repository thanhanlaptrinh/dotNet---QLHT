using Microsoft.Data.SqlClient;
using QuanLiHocTap.Data;
using QuanLiHocTap.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media.Effects;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QuanLiHocTap.Repository
{
    public class EnrollmentRepository
    {
        public int Count_Enrollment()
        {
            int count = 0;
            string query = "SELECT COUNT(DISTINCT EnrollmentID) FROM Enrollments";
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

        public int Count_Credits()
        {
            int count = 0;
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = @"
                        SELECT ISNULL(SUM(c.Credits), 0) 
                        FROM Enrollments e
                        JOIN Offerings o ON e.OfferingID = o.OfferingID
                        JOIN Courses c ON o.CourseID = c.CourseID
                        WHERE e.StudentID = @StudentID AND e.Status = 'Enrolled'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", GlobalVariable.UserID);
                    try
                    {
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                return count;
            }
        }

        public ObservableCollection<RecentRegistrationModel> GetRecent()
        {
            ObservableCollection<RecentRegistrationModel> recentRegistrations = new ObservableCollection<RecentRegistrationModel>();
            string query = @"
                        SELECT TOP 5 u.FullName, c.CourseName, e.EnrollDate, e.Status
                        FROM Enrollments e
                        JOIN Students s ON e.StudentID = s.StudentID
                        JOIN Users u ON s.StudentID = u.UserID
                        JOIN Offerings o ON e.OfferingID = o.OfferingID
                        JOIN Courses c ON o.CourseID = c.CourseID
                        ORDER BY e.EnrollDate DESC";
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string rawStatus = reader["Status"].ToString();
                            string friendlyStatus = rawStatus == "Enrolled" ? "Đã đăng ký" : "Chờ xác nhận";

                            recentRegistrations.Add(new RecentRegistrationModel
                            {
                                StudentName = reader["FullName"].ToString(),
                                CourseName = reader["CourseName"].ToString(),
                                RegistrationDate = Convert.ToDateTime(reader["EnrollDate"]),
                                Status = friendlyStatus
                            });
                        }
                        return recentRegistrations;
                    }
                }
            }
        }
    }
}
