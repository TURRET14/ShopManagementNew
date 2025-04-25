using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ShopManagement.UpdateTables
{
    public partial class UpdateEmployee : UserControl
    {
        private bool IsCurrentUser = false;
        public event Events.ShowMessageDelegate ShowMessageEvent;
        public event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public UpdateEmployee(Events.ShowAnotherTabDelegate ShowAnotherTab, Employee Selected, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            if (Selected.UserLogin == UserData.Login)
            {
                IsCurrentUser = true;
            }
            ShowAnotherTabEvent = ShowAnotherTab;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            ColumnGender.ItemsSource = new List<string>() { "M", "F"};
            switch (Selected.Position)
            {
                case "SYSTEM_ADMIN":
                    Selected.Position = "Системный Администратор";
                    break;
                case "SHOP_ADMIN":
                    Selected.Position = "Администратор";
                    break;
                case "SHOP_MANAGER":
                    Selected.Position = "Менеджер";
                    break;
                case "SHOP_CASHIER":
                    Selected.Position = "Кассир";
                    break;
            }
            ColumnPosition.ItemsSource = new List<string>() {"Системный Администратор", "Администратор", "Менеджер", "Кассир" };
            DataGrid_Table.ItemsSource = new List<Employee> { Selected };
        }

        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Employee Selected = ((List<Employee>)DataGrid_Table.ItemsSource)[0];

                if (Selected.Name is not null)
                {
                    if (Selected.Name.Length > 100)
                    {
                        ShowMessageEvent("Ошибка Записи", "Длина Имени Не Может Быть Больше 100 Символов!");
                        return;
                    }
                    else if (Selected.Name.Length == 0)
                    {
                        ShowMessageEvent("Ошибка Записи", "Имя Не Может Быть Пустым!");
                        return;
                    }
                }
                else
                {
                    ShowMessageEvent("Ошибка Записи", "Имя Не Может Быть Пустым!");
                    return;
                }

                if (Selected.PhoneNumber is not null)
                {
                    if (Selected.PhoneNumber.Length > 20)
                    {
                        ShowMessageEvent("Ошибка Записи", "Длина Номера Телефона Не Может Быть Больше 20 Символов!");
                        return;
                    }
                    else if (Selected.PhoneNumber.Length == 0)
                    {
                        Selected.PhoneNumber = null;
                    }
                }

                if (Selected.Email is not null)
                {
                    if (Selected.Email.Length > 100)
                    {
                        ShowMessageEvent("Ошибка Записи", "Длина Электронной Почты Не Может Быть Больше 100 Символов!");
                        return;
                    }
                    else if (Selected.Email.Length == 0)
                    {
                        Selected.PhoneNumber = null;
                    }
                }

                if (Selected.Gender is not null)
                {
                    if (Selected.Gender.Length == 0)
                    {
                        Selected.Gender = null;
                    }
                    else if (Selected.Gender != "M" && Selected.Gender != "F")
                    {
                        ShowMessageEvent("Ошибка Записи", "Пол Может Быть Только Мужской (M) Или Женский (F)!");
                        return;
                    }
                }

                if (Selected.Experience < 0)
                {
                    ShowMessageEvent("Ошибка Записи", "Опыт Не Может Быть Меньше 0!");
                    return;
                }

                if (Selected.Salary < 0)
                {
                    ShowMessageEvent("Ошибка Записи", "Зарплата Не Может Быть Меньше 0!");
                    return;
                }

                switch (Selected.Position)
                {
                    case "Системный Администратор":
                        Selected.Position = "SYSTEM_ADMIN";
                        break;
                    case "Администратор":
                        Selected.Position = "SHOP_ADMIN";
                        break;
                    case "Менеджер":
                        Selected.Position = "SHOP_MANAGER";
                        break;
                    case "Кассир":
                        Selected.Position = "SHOP_CASHIER";
                        break;
                    default:
                        ShowMessageEvent("Ошибка Записи", "Указана недопустимая должность!");
                        return;
                }

                if (Selected.UserPassword is not null)
                {
                    if (Selected.UserPassword.Length > 50)
                    {
                        ShowMessageEvent("Ошибка Записи", "Длина Пароля Не Может Быть Больше 50 Символов!");
                        return;
                    }
                    if (Selected.UserPassword.Length == 0)
                    {
                        Selected.UserPassword = null;
                    }
                }

                if (Selected.UserLogin is not null)
                {
                    if (Selected.UserLogin.Length > 50)
                    {
                        ShowMessageEvent("Ошибка Записи", "Длина Логина Не Может Быть Больше 50 Символов!");
                        return;
                    }
                    if (Selected.UserLogin.Length == 0)
                    {
                        ShowMessageEvent("Ошибка Записи", "Логин Не Может Быть Пустым!");
                        return;
                    }
                }
                else
                {
                    ShowMessageEvent("Ошибка Записи", "Логин Не Может Быть Пустым!");
                    return;
                }

                if (CheckBox_ChangePassword.IsChecked == false)
                {
                    Selected.UserPassword = null;
                }

                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.UpdateEmployee @ID = {0}, @Name = {1},  @Age = {2}, @Gender = {3}, @PhoneNumber = {4}, @Email = {5}, @Experience = {6}, @Position = {7}, @Salary = {8}, @UserLogin = {9}, @UserPassword = {10}, @ChangePassword = {11}, @AdminLogin = {12}, @AdminPassword = {13}", Selected.Id, Selected.Name, Selected.Age, Selected.Gender, Selected.PhoneNumber, Selected.Email, Selected.Experience, Selected.Position, Selected.Salary, Selected.UserLogin, Selected.UserPassword, CheckBox_ChangePassword.IsChecked, UserData.Login, UserData.Password);
                ShowAnotherTabEvent.Invoke(new Tables.EmployeesTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));

                if (IsCurrentUser == true && CheckBox_ChangePassword.IsChecked == true)
                {
                    UserData.Login = Selected.UserLogin;
                    UserData.Password = Selected.UserPassword;
                }
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Employee Selected = ((List<Employee>)DataGrid_Table.ItemsSource)[0];
                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.DeleteEmployee @ID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", Selected.Id, UserData.Login, UserData.Password);
                ShowAnotherTabEvent.Invoke(new Tables.EmployeesTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.EmployeesTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
        }
    }
}
