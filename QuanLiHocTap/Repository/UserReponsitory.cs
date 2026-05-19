using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Microsoft.Data.SqlClient;
using QuanLiHocTap.Helper;

namespace QuanLiHocTap.Repository
{
    public class UserReponsitory
    {
        public int Login(string username, string password)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                try
                {
                    string query = "SELECT Role, UserID, FullName FROM Users WHERE Username=@UserName AND PasswordHash=@Password";
                    SqlCommand cmd = new SqlCommand(query,con);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    con.Open();
                    var result = cmd.ExecuteReader();
                    if (result.Read())
                    {
                        GlobalVariable.UserID = Convert.ToInt32(result["UserID"]);
                        GlobalVariable.Username = result["FullName"].ToString();
                        GlobalVariable.Role = result["Role"].ToString();
                        return 1;
                    }
                    else
                    {
                        MessageBox.Show("Tài khoản không tồn tại!");
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return 0;
                }
            }
        }
    }
}
