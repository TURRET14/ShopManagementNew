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

namespace ShopManagement.Tables
{
    public partial class CustomerOrdersTable : UserControl
    {
        public event Events.ShowMessageDelegate ShowMessageEvent;
        public event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public CustomerOrdersTable(Events.ShowAnotherTabDelegate ShowAnotherTab, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            try
            {
                List<CustomerOrder> Orders = ShopManagementContext.GetContext().CustomerOrders.FromSqlRaw("EXEC GetCustomerOrders @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                List<Customer> Customers = ShopManagementContext.GetContext().Customers.FromSqlRaw("EXEC GetCustomers @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                List<Employee> Employees = ShopManagementContext.GetContext().Employees.FromSqlRaw("EXEC GetEmployeesIDAndNames @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                foreach (CustomerOrder Order in Orders)
                {
                    Order.Customer = Customers.FirstOrDefault(Entry => Entry.Id == Order.CustomerId);
                    Order.Employee = Employees.FirstOrDefault(Entry => Entry.Id == Order.EmployeeId);
                }
                DataGrid_Table.ItemsSource = Orders;
            }
            catch (SqlException Ex)
            {
                if (Ex.Message == "AUTHORIZATION_ERROR")
                {
                    ShowMessageEvent.Invoke("ERROR", "AUTHORIZATION_ERROR");
                    ShowLoginPageEvent.Invoke();
                }
                else
                {
                    ShowMessageEvent.Invoke("ERROR", "UNKNOWN_SQL_SERVER_ERROR. Error Code: " + Ex.ErrorCode);
                }
            }
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new InsertIntoTables.CreateCustomerOrder(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
        }

        private void DataGrid_Table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGrid_Table.SelectedItem != null)
            {
                ShowAnotherTabEvent.Invoke(new Tables.CustomerOrderItemsTable(ShowAnotherTabEvent, (CustomerOrder)DataGrid_Table.SelectedItem, ShowMessageEvent, ShowLoginPageEvent));
            }
        }
    }
}
