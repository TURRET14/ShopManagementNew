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
    public partial class CustomerReturnItemsTable : UserControl
    {
        public event Events.ShowMessageDelegate ShowMessageEvent;
        public event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        private CustomerOrderItem OrderItem;
        private CustomerOrder Order;
        public CustomerReturnItemsTable(Events.ShowAnotherTabDelegate ShowAnotherTab, CustomerOrderItem OrderItemObject, CustomerOrder OrderObject, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            OrderItem = OrderItemObject;
            Order = OrderObject;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            DataGrid_Header.ItemsSource = new List<CustomerOrderItem>() { OrderItem };
            try
            {
                List<CustomerReturnItem> Returns = ShopManagementContext.GetContext().CustomerReturnItems.FromSqlRaw("EXEC GetCustomerReturnItems @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().Where(Entry => Entry.OrderItemId == OrderItem.Id).ToList();
                List<Employee> Employees = ShopManagementContext.GetContext().Employees.FromSqlRaw("EXEC GetEmployeesIDAndNames @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                foreach (CustomerReturnItem ReturnItem in Returns)
                {
                    ReturnItem.Employee = Employees.FirstOrDefault(Entry => Entry.Id == ReturnItem.EmployeeId);
                }
                DataGrid_Table.ItemsSource = Returns;
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
            ShowAnotherTabEvent.Invoke(new InsertIntoTables.CreateCustomerReturnItem(ShowAnotherTabEvent, OrderItem, Order, ShowMessageEvent, ShowLoginPageEvent));
        }

        private void DataGrid_Table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGrid_Table.SelectedItem != null)
            {
                ShowAnotherTabEvent.Invoke(new UpdateTables.UpdateCustomerReturnItem(ShowAnotherTabEvent, (CustomerReturnItem)DataGrid_Table.SelectedItem, OrderItem, Order, ShowMessageEvent, ShowLoginPageEvent));
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.DeleteCustomerOrderItem @ID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", OrderItem.Id, UserData.Login, UserData.Password);
                ShowAnotherTabEvent.Invoke(new Tables.CustomerOrderItemsTable(ShowAnotherTabEvent, Order, ShowMessageEvent, ShowLoginPageEvent));
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
                else
                {
                    ShowMessageEvent.Invoke("ERROR", "UNKNOWN_SQL_SERVER_ERROR. Error Code: " + Ex.ErrorCode);
                }
            }
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.CustomerOrderItemsTable(ShowAnotherTabEvent, Order, ShowMessageEvent, ShowLoginPageEvent));
        }
    }
}
