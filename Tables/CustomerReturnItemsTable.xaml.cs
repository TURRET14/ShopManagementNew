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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShopManagement.Tables
{
    public partial class CustomerReturnItemsTable : UserControl
    {
        private List<CustomerReturnItem> CustomerReturnItemsList;
        private event Events.ShowMessageDelegate ShowMessageEvent;
        private event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        private event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        private CustomerOrderItem OrderItem;
        private CustomerOrder Order;
        public CustomerReturnItemsTable(Events.ShowAnotherTabDelegate ShowAnotherTab, CustomerOrderItem OrderItemObject, CustomerOrder OrderObject, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            if (UserData.AccessLevel != "SYSTEM_ADMIN" && UserData.AccessLevel != "SHOP_ADMIN")
            {
                ActionsPanel.Visibility = Visibility.Collapsed;
            }
            if (UserData.AccessLevel != "SYSTEM_ADMIN" && UserData.AccessLevel != "SHOP_ADMIN" && UserData.AccessLevel != "SHOP_MANAGER")
            {
                Button_Create.Visibility = Visibility.Collapsed;
            }
            ShowAnotherTabEvent = ShowAnotherTab;
            OrderItem = OrderItemObject;
            Order = OrderObject;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            DataGrid_Header.ItemsSource = new List<CustomerOrderItem>() { OrderItem };
            try
            {
                CustomerReturnItemsList = ShopManagementContext.GetContext().CustomerReturnItems.FromSqlRaw("EXEC GetCustomerReturnItems @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().Where(Entry => Entry.OrderItemId == OrderItem.Id).ToList();
                List<Employee> Employees = ShopManagementContext.GetContext().Employees.FromSqlRaw("EXEC GetEmployeesIDAndNames @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                foreach (CustomerReturnItem ReturnItem in CustomerReturnItemsList)
                {
                    ReturnItem.Employee = Employees.FirstOrDefault(Entry => Entry.Id == ReturnItem.EmployeeId);
                    ReturnItem.OrderItem = OrderItem;
                }
                DataGrid_Table.ItemsSource = CustomerReturnItemsList;
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
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
            ConfirmationMessage Msg = new ConfirmationMessage("Удаление", "Вы уверены, что хотите удалить запись?");
            Msg.PlacementTarget = ActionsPanel;
            Msg.IsOpen = true;
            void Confirm(object sender, RoutedEventArgs e)
            {
                try
                {
                    Msg.IsOpen = false;
                    ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.DeleteCustomerOrderItem @ID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", OrderItem.Id, UserData.Login, UserData.Password);
                    ShowAnotherTabEvent.Invoke(new Tables.CustomerOrderItemsTable(ShowAnotherTabEvent, Order, ShowMessageEvent, ShowLoginPageEvent));
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
            ShowAnotherTabEvent.Invoke(new Tables.CustomerOrderItemsTable(ShowAnotherTabEvent, Order, ShowMessageEvent, ShowLoginPageEvent));
        }

        private void Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            List<CustomerReturnItem> FilteredList = new List<CustomerReturnItem>(CustomerReturnItemsList);
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
            if (CheckBox_Amount.IsChecked == true)
            {
                try
                {
                    if (TextBox_AmountBegin.Text.Length > 0)
                    {
                        FilteredList = FilteredList.Where(Entry => Entry.Amount >= Convert.ToInt32(TextBox_AmountBegin.Text)).ToList();
                    }
                }
                catch { }
                try
                {
                    if (TextBox_AmountEnd.Text.Length > 0)
                    {
                        FilteredList = FilteredList.Where(Entry => Entry.Amount <= Convert.ToInt32(TextBox_AmountEnd.Text)).ToList();
                    }
                }
                catch { }
            }
            if (CheckBox_Reason.IsChecked == true)
            {
                if (TextBox_Reason.Text.Length > 0)
                {
                    FilteredList = FilteredList.Where(Entry => Entry.Reason.ToLower().Contains(TextBox_Reason.Text.ToLower())).ToList();
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
