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
    public partial class UpdateProduct : UserControl
    {
        private event Events.ShowMessageDelegate ShowMessageEvent;
        private event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        private event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public UpdateProduct(Events.ShowAnotherTabDelegate ShowAnotherTab, Product Selected, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            if (UserData.AccessLevel != "SYSTEM_ADMIN" && UserData.AccessLevel != "SHOP_ADMIN")
            {
                ActionsPanel.Visibility = Visibility.Collapsed;
            }
            ShowAnotherTabEvent = ShowAnotherTab;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            DataGrid_Table.ItemsSource = new List<Product>() { Selected };
        }

        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product Selected = ((List<Product>)DataGrid_Table.ItemsSource)[0];

                if (Selected.Name is not null)
                {
                    if (Selected.Name.Length > 100)
                    {
                        ShowMessageEvent("Ошибка Записи", "Длина Названия Товара Не Может Быть Больше 100 Символов!");
                        return;
                    }
                    else if (Selected.Name.Length == 0)
                    {
                        ShowMessageEvent("Ошибка Записи", "Название Товара Не Может Быть Пустым!");
                        return;
                    }
                }
                else
                {
                    ShowMessageEvent("Ошибка Записи", "Название Товара Не Может Быть Пустым!");
                    return;
                }

                if (Selected.Price < 0)
                {
                    ShowMessageEvent("Ошибка Записи", "Цена Товара Не Может Быть Меньше 0!");
                    return;
                }

                if (Selected.Amount < 0)
                {
                    ShowMessageEvent("Ошибка Записи", "Количество Товара Не Может Быть Меньше 0!");
                    return;
                }

                if (Selected.Description is not null)
                {
                    if (Selected.Description.Length > 150)
                    {
                        ShowMessageEvent("Ошибка Записи", "Длина Описания Товара Не Может Быть Больше 100 Символов!");
                        return;
                    }
                    else if (Selected.Description.Length == 0)
                    {
                        Selected.Description = null;
                    }
                }

                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.UpdateProduct @ID = {0}, @Name = {1},  @Price = {2}, @Amount = {3}, @Description = {4}, @AdminLogin = {5}, @AdminPassword = {6}", Selected.Id, Selected.Name, Selected.Price, Selected.Amount, Selected.Description, UserData.Login, UserData.Password);
                ShowAnotherTabEvent.Invoke(new Tables.ProductsTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
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
                    Product Selected = ((List<Product>)DataGrid_Table.ItemsSource)[0];
                    ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.DeleteProduct @ID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", Selected.Id, UserData.Login, UserData.Password);
                    ShowAnotherTabEvent.Invoke(new Tables.ProductsTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
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
            ShowAnotherTabEvent.Invoke(new Tables.ProductsTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
        }
    }
}
