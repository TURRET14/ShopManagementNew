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
    public partial class CreateCustomerOrder: UserControl
    {
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public CreateCustomerOrder(Events.ShowAnotherTabDelegate ShowAnotherTab)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            ColumnCustomer.ItemsSource = ShopManagementContext.GetContext().Customers.FromSqlRaw("EXEC GetCustomers @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList().OrderBy(Row => Row.Name);
            DataGrid_Table.ItemsSource = new List<CustomerOrder>() { new CustomerOrder()};
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            CustomerOrder Selected = ((List<CustomerOrder>)DataGrid_Table.ItemsSource)[0];
            ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.CreateCustomerOrder @CustomerID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", Selected.Customer.Id, UserData.Login, UserData.Password);
            ShowAnotherTabEvent.Invoke(new Tables.CustomerOrdersTable(ShowAnotherTabEvent));
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.CustomerOrdersTable(ShowAnotherTabEvent));
        }
    }
}
