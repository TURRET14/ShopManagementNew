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
        private List<CustomerOrder> OrdersList;
        private event Events.ShowMessageDelegate ShowMessageEvent;
        private event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        private event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public CustomerOrdersTable(Events.ShowAnotherTabDelegate ShowAnotherTab, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            try
            {
                OrdersList = ShopManagementContext.GetContext().CustomerOrders.FromSqlRaw("EXEC GetCustomerOrders @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                List<CustomerOrderItem> OrderItems = ShopManagementContext.GetContext().CustomerOrderItems.FromSqlRaw("EXEC GetCustomerOrderItems @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                List<Customer> Customers = ShopManagementContext.GetContext().Customers.FromSqlRaw("EXEC GetCustomers @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                List<Employee> Employees = ShopManagementContext.GetContext().Employees.FromSqlRaw("EXEC GetEmployeesIDAndNames @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                foreach (CustomerOrder Order in OrdersList)
                {
                    Order.Customer = Customers.FirstOrDefault(Entry => Entry.Id == Order.CustomerId);
                    Order.Employee = Employees.FirstOrDefault(Entry => Entry.Id == Order.EmployeeId);
                    Order.CustomerOrderItems = OrderItems.Where(Entry => Entry.OrderId == Order.Id).ToList();
                }
                DataGrid_Table.ItemsSource = OrdersList;
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
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

        private void Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            List<CustomerOrder> FilteredList = new List<CustomerOrder>(OrdersList);
            if (CheckBox_CustomerName.IsChecked == true)
            {
                if (TextBox_CustomerName.Text.Length > 0)
                {
                    FilteredList = FilteredList.Where(Entry => {
                        if (Entry.Customer is not null)
                        {
                            return Entry.Customer.Name.ToLower().Contains(TextBox_CustomerName.Text.ToLower());
                        }
                        else
                        {
                            return false;
                        }
                        }).ToList();
                }
            }
            if (CheckBox_EmployeeName.IsChecked == true)
            {
                if (TextBox_EmployeeName.Text.Length > 0)
                {
                    FilteredList = FilteredList.Where(Entry => {
                        if (Entry.Employee is not null)
                        {
                            return Entry.Employee.Name.ToLower().Contains(TextBox_EmployeeName.Text.ToLower());
                        }
                        else
                        {
                            return false;
                        }
                    }).ToList();
                }
            }
            if (CheckBox_Date.IsChecked == true)
            {
                if (DatePicker_DateBegin.SelectedDate is not null)
                {
                    FilteredList = FilteredList.Where(Entry => Entry.Date.DateTime >= DatePicker_DateBegin.SelectedDate).ToList();
                }
                if (DatePicker_DateEnd.SelectedDate is not null)
                {
                    FilteredList = FilteredList.Where(Entry => Entry.Date.DateTime <= DatePicker_DateEnd.SelectedDate.Value.AddDays(1)).ToList();
                }
            }
            DataGrid_Table.ItemsSource = FilteredList;
        }
    }
}
