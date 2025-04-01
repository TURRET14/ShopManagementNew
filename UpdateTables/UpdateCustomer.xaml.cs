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
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public UpdateCustomer(Events.ShowAnotherTabDelegate ShowAnotherTab, Customer Selected)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            DataGrid_Table.ItemsSource = new List<Customer>() { Selected };
        }

        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            Customer Selected = ((List<Customer>)DataGrid_Table.ItemsSource)[0];
            ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.UpdateCustomer @ID = {0}, @Name = {1},  @PhoneNumber = {2}, @Email = {3},  @AdminLogin = {4}, @AdminPassword = {5}", Selected.Id, Selected.Name, Selected.PhoneNumber, Selected.Email, UserData.Login, UserData.Password);
            ShowAnotherTabEvent.Invoke(new Tables.CustomersTable(ShowAnotherTabEvent));
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            Customer Selected = ((List<Customer>)DataGrid_Table.ItemsSource)[0];
            ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.DeleteCustomer @ID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", Selected.Id, UserData.Login, UserData.Password);
            ShowAnotherTabEvent.Invoke(new Tables.CustomersTable(ShowAnotherTabEvent));
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.CustomersTable(ShowAnotherTabEvent));
        }
    }
}
