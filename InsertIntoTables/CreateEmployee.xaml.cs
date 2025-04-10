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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShopManagement.InsertIntoTables
{
    public partial class CreateEmployee : UserControl
    {
        public event Events.ShowMessageDelegate ShowMessageEvent;
        public event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public CreateEmployee(Events.ShowAnotherTabDelegate ShowAnotherTab, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            ColumnGender.ItemsSource = new List<string>() { "M", "F" };
            ColumnPosition.ItemsSource = new List<string>() { "SHOP_ADMIN", "SHOP_MANAGER", "SHOP_CASHIER" };
            DataGrid_Table.ItemsSource = new List<Employee>() { new Employee()};
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
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

                if (Selected.Age < 18 || Selected.Age > 100)
                {
                    ShowMessageEvent("Ошибка Записи", "Возраст Не Может Быть Меньше 18 И Больше 100!");
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

                if (Selected.Position != "SYSTEM_ADMIN" && Selected.Position != "SHOP_ADMIN" && Selected.Position != "SHOP_MANAGER" && Selected.Position != "SHOP_CASHIER")
                {
                    ShowMessageEvent("Ошибка Записи", "Такой Должности Нет!");
                    return;
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
                        Selected.UserLogin = null;
                    }
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

                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.CreateEmployee @Name = {0},  @Age = {1}, @Gender = {2}, @PhoneNumber = {3}, @Email = {4}, @Experience = {5}, @Position = {6}, @UserLogin = {7}, @UserPassword = {8}, @AdminLogin = {9}, @AdminPassword = {10}", Selected.Name, Selected.Age, Selected.Gender, Selected.PhoneNumber, Selected.Email, Selected.Experience, Selected.Position, Selected.UserLogin, Selected.UserPassword, UserData.Login, UserData.Password);
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
