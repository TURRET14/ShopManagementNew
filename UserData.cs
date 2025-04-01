using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement
{
    public class UserData
    {
        static public string? AccessLevel { get; set; }
        static public string? Login { get; set; }
        static public string? Password { get; set; }
        static public void ClearData()
        {
            AccessLevel = null;
            Login = null;
            Password = null;
        }
    }
}
