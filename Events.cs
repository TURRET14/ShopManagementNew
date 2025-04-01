using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ShopManagement
{
    public class Events
    {
        public delegate void ShowMessageDelegate(string Header, string Text);
        public delegate void ShowLoginPageDelegate();
        public delegate void ShowMainPageDelegate();
        public delegate void ShowAnotherTabDelegate(UserControl Tab);
    }
}
