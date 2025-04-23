using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShopManagement
{
    public partial class LoginPage : UserControl
    {
        public event Events.ShowMessageDelegate ShowMessageEvent;
        public event Events.ShowMainPageDelegate ShowMainPageEvent;

        public LoginPage(Events.ShowMessageDelegate ShowMessage, Events.ShowMainPageDelegate ShowMainPage)
        {
            InitializeComponent();
            ShowMessageEvent = ShowMessage;
            ShowMainPageEvent = ShowMainPage;
        }

        private void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TextBox_Login.Text.Length > 50 || PasswordBox_Password.Password.Length > 50)
                {
                    ShowMessageEvent.Invoke("Ошибка", "Пароль или логин слишком длинные!");
                    return;
                }
                if (TextBox_Login.Text.Length == 0 || PasswordBox_Password.Password.Length == 0)
                {
                    ShowMessageEvent.Invoke("Ошибка", "Пароль или логин не могут быть пустыми!");
                    return;
                }
                string Login = TextBox_Login.Text;
                string Password = PasswordBox_Password.Password;
                string AccessLevel = Models.ShopManagementContext.GetContext().Database.SqlQueryRaw<string>("SELECT Dbo.SignIn (@p0, @p1)", Login, Password).AsEnumerable().First();
                if (AccessLevel == "SYSTEM_ADMIN" || AccessLevel == "SHOP_ADMIN" || AccessLevel == "SHOP_MANAGER" || AccessLevel == "SHOP_CASHIER")
                {
                    UserData.AccessLevel = AccessLevel;
                    UserData.Login = Login;
                    UserData.Password = Password;
                    ShowMainPageEvent.Invoke();
                }
                else
                {
                    ShowMessageEvent("Ошибка", "Неверный логин или пароль!");
                }
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, () => { });
            }
        }
    }
}
