using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace QuanLiHocTap.Helper
{
    public static class GlobalVariable
    {
        public static int UserID { get; set; } = 0;
        public static string Username { get; set; }
        public static string Role { get; set; }
    }
}
