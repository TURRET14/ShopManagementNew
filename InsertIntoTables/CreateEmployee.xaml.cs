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
        private event Events.ShowMessageDelegate ShowMessageEvent;
        private event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        private event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public CreateEmployee(Events.ShowAnotherTabDelegate ShowAnotherTab, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            ColumnGender.ItemsSource = new List<string>() { "M", "F", "Нет" };
            ColumnPosition.ItemsSource = new List<string>() { "Администратор", "Менеджер", "Кассир", "Нет" };
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
                        ShowMessageEvent("Ошибка Записи", "Длина имени не может быть больше 100 символов!");
                        return;
                    }
                    else if (Selected.Name.Length == 0)
                    {
                        ShowMessageEvent("Ошибка Записи", "Имя не может быть пустым!");
                        return;
                    }
                }
                else
                {
                    ShowMessageEvent("Ошибка Записи", "Имя не может быть пустым!");
                    return;
                }

                if (Selected.Age < 18 || Selected.Age > 100)
                {
                    ShowMessageEvent("Ошибка Записи", "Возраст не может быть меньше 18 и больше 100!");
                    return;
                }

                if (Selected.PhoneNumber is not null)
                {
                    if (Selected.PhoneNumber.Length > 20)
                    {
                        ShowMessageEvent("Ошибка Записи", "Длина номера телефона не может быть больше 20 символов!");
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
                        ShowMessageEvent("Ошибка Записи", "Длина электронной почты не может быть больше 100 символов!");
                        return;
                    }
                    else if (Selected.Email.Length == 0)
                    {
                        Selected.PhoneNumber = null;
                    }
                }

                if (Selected.Gender is not null)
                {
                    if (Selected.Gender.Length == 0 || Selected.Gender == "Нет")
                    {
                        Selected.Gender = null;
                    }
                    else if (Selected.Gender != "M" && Selected.Gender != "F")
                    {
                        ShowMessageEvent("Ошибка Записи", "Пол может быть только мужской (M) или женский (F)!");
                        return;
                    }
                }

                if (Selected.Experience < 0)
                {
                    ShowMessageEvent("Ошибка Записи", "Опыт не может быть меньше 0!");
                    return;
                }

                if (Selected.Salary < 0)
                {
                    ShowMessageEvent("Ошибка Записи", "Зарплата не может быть меньше 0!");
                    return;
                }

                string? Position = null;

                switch (Selected.Position)
                {
                    case "Администратор":
                        Position = "SHOP_ADMIN";
                        break;
                    case "Менеджер":
                        Position = "SHOP_MANAGER";
                        break;
                    case "Кассир":
                        Position = "SHOP_CASHIER";
                        break;
                }

                if (Selected.UserLogin is not null)
                {
                    if (Selected.UserLogin.Length > 50)
                    {
                        ShowMessageEvent("Ошибка Записи", "Длина логина не может быть больше 50 символов!");
                        return;
                    }
                    else if (Selected.UserLogin.Length == 0)
                    {
                        ShowMessageEvent("Ошибка Записи", "Логин не может быть пустым!");
                        return;
                    }
                }
                else
                {
                    ShowMessageEvent("Ошибка Записи", "Логин не может быть пустым!");
                    return;
                }

                if (Selected.UserPassword is not null)
                {
                    if (Selected.UserPassword.Length > 50)
                    {
                        ShowMessageEvent("Ошибка Записи", "Длина пароля не может быть больше 50 символов!");
                        return;
                    }
                    else if (Selected.UserPassword.Length == 0)
                    {
                        ShowMessageEvent("Ошибка Записи", "Пароль не может быть пустым!");
                        return;
                    }
                }
                else
                {
                    ShowMessageEvent("Ошибка Записи", "Пароль не может быть пустым!");
                    return;
                }

                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.CreateEmployee @Name = {0},  @Age = {1}, @Gender = {2}, @PhoneNumber = {3}, @Email = {4}, @Experience = {5}, @Position = {6}, @Salary = {7}, @UserLogin = {8}, @UserPassword = {9}, @AdminLogin = {10}, @AdminPassword = {11}", Selected.Name, Selected.Age, Selected.Gender, Selected.PhoneNumber, Selected.Email, Selected.Experience, Position, Selected.Salary, Selected.UserLogin, Selected.UserPassword, UserData.Login, UserData.Password);
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
