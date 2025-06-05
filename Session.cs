using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computer_Shop_System
{
    public static class Session
    {
        public static int UserId { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string Permission { get; set; }
        public static string Email { get; set; }
        public static string PhoneNumber { get; set; }
    }
}
