using System;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace QuanLiHocTap.Helper
{
    public static class DatabaseHelper
    {
        private static string connectionString;
        private static string GetConnectionString()
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    var settings = ConfigurationManager.ConnectionStrings["AcademicCon"];
                    if (settings == null)
                    {
                        throw new Exception("Không tìm thấy chuỗi kết nối mang tên 'AcademicCon' trong file App.config!");
                    }

                    connectionString = settings.ConnectionString;
                }
                return connectionString;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cấu hình hệ thống: {ex.Message}", ex);
                return null;
            }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}