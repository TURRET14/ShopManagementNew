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
    public partial class UpdateSupplier : UserControl
    {
        public event Events.ShowMessageDelegate ShowMessageEvent;
        public event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public UpdateSupplier(Events.ShowAnotherTabDelegate ShowAnotherTab, Supplier Selected, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            DataGrid_Table.ItemsSource = new List<Supplier>() { Selected };
        }

        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Supplier Selected = ((List<Supplier>)DataGrid_Table.ItemsSource)[0];
                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.UpdateSupplier @ID = {0}, @Name = {1},  @PhoneNumber = {2}, @Email = {3}, @AccountNumber = {4},  @AdminLogin = {5}, @AdminPassword = {6}", Selected.Id, Selected.Name, Selected.PhoneNumber, Selected.Email, Selected.AccountNumber, UserData.Login, UserData.Password);
                ShowAnotherTabEvent.Invoke(new Tables.SuppliersTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
            }
            catch (SqlException Ex)
            {
                if (Ex.Message == "AUTHORIZATION_ERROR")
                {
                    ShowMessageEvent.Invoke("ERROR", "AUTHORIZATION_ERROR");
                    ShowLoginPageEvent.Invoke();
                }
                else if (Ex.Message == "INVALID_ID")
                {
                    ShowMessageEvent.Invoke("ERROR", "INVALID_ID");
                    ShowLoginPageEvent.Invoke();
                }
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Supplier Selected = ((List<Supplier>)DataGrid_Table.ItemsSource)[0];
                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.DeleteSupplier @ID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", Selected.Id, UserData.Login, UserData.Password);
                ShowAnotherTabEvent.Invoke(new Tables.SuppliersTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
            }
            catch (SqlException Ex)
            {
                if (Ex.Message == "AUTHORIZATION_ERROR")
                {
                    ShowMessageEvent.Invoke("ERROR", "AUTHORIZATION_ERROR");
                    ShowLoginPageEvent.Invoke();
                }
                else if (Ex.Message == "INVALID_ID")
                {
                    ShowMessageEvent.Invoke("ERROR", "INVALID_ID");
                    ShowLoginPageEvent.Invoke();
                }
            }
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.SuppliersTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
        }
    }
}
