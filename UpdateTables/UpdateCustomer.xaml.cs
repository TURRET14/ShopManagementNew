﻿using Microsoft.Data.SqlClient;
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
    public partial class UpdateCustomer : UserControl
    {
        private event Events.ShowMessageDelegate ShowMessageEvent;
        private event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        private event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public UpdateCustomer(Events.ShowAnotherTabDelegate ShowAnotherTab, Customer Selected, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            if (UserData.AccessLevel != "SYSTEM_ADMIN" && UserData.AccessLevel != "SHOP_ADMIN")
            {
                ActionsPanel.Visibility = Visibility.Collapsed;
            }
            ShowAnotherTabEvent = ShowAnotherTab;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            DataGrid_Table.ItemsSource = new List<Customer>() { Selected };
        }

        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer Selected = ((List<Customer>)DataGrid_Table.ItemsSource)[0];

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

                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.UpdateCustomer @ID = {0}, @Name = {1},  @PhoneNumber = {2}, @Email = {3},  @AdminLogin = {4}, @AdminPassword = {5}", Selected.Id, Selected.Name, Selected.PhoneNumber, Selected.Email, UserData.Login, UserData.Password);
                ShowAnotherTabEvent.Invoke(new Tables.CustomersTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationMessage Msg = new ConfirmationMessage("Удаление", "Вы уверены, что хотите удалить запись?");
            Msg.PlacementTarget = Button_Delete;
            Msg.IsOpen = true;
            void Confirm(object sender, RoutedEventArgs e)
            {
                try
                {
                    Msg.IsOpen = false;
                    Customer Selected = ((List<Customer>)DataGrid_Table.ItemsSource)[0];
                    ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.DeleteCustomer @ID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", Selected.Id, UserData.Login, UserData.Password);
                    ShowAnotherTabEvent.Invoke(new Tables.CustomersTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
                }
                catch (SqlException Ex)
                {
                    ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
                }
            }
            void Deny(object sender, RoutedEventArgs e)
            {
                try
                {
                    Msg.IsOpen = false;
                }
                catch (SqlException Ex)
                {
                    ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
                }
            }
            Msg.Button_Yes.Click += new RoutedEventHandler(Confirm);
            Msg.Button_No.Click += new RoutedEventHandler(Deny);
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.CustomersTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
        }
    }
}
