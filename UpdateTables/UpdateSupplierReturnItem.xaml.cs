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
    public partial class UpdateSupplierReturnItem : UserControl
    {
        private event Events.ShowMessageDelegate ShowMessageEvent;
        private event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        private event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        
        private SupplierOrderItem OrderItem;
        private SupplierOrder Order;
        public UpdateSupplierReturnItem(Events.ShowAnotherTabDelegate ShowAnotherTab, SupplierReturnItem Selected, SupplierOrderItem OrderItemObject, SupplierOrder OrderObject, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            if (UserData.AccessLevel != "SYSTEM_ADMIN" && UserData.AccessLevel != "SHOP_ADMIN")
            {
                ActionsPanel.Visibility = Visibility.Collapsed;
            }
            ShowAnotherTabEvent = ShowAnotherTab;
            OrderItem = OrderItemObject;
            Order = OrderObject;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;

            DataGrid_Header.ItemsSource = new List<SupplierOrderItem> { OrderItem };
            try
            {
                List<Employee> Employees = ShopManagementContext.GetContext().Employees.FromSqlRaw("EXEC GetEmployeesIDAndNames @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                Selected.Employee = Employees.FirstOrDefault(Entry => Entry.Id == Selected.EmployeeId);
                Selected.OrderItem = OrderItem;
                DataGrid_Table.ItemsSource = new List<SupplierReturnItem>() { Selected };
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void ActionsPanel_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationMessage Msg = new ConfirmationMessage("Удаление", "Вы уверены, что хотите удалить запись?");
            Msg.PlacementTarget = ActionsPanel;
            Msg.IsOpen = true;
            void Confirm(object sender, RoutedEventArgs e)
            {
                try
                {
                    Msg.IsOpen = false;
                    SupplierReturnItem Selected = ((List<SupplierReturnItem>)DataGrid_Table.ItemsSource)[0];
                    ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.DeleteSupplierReturnItem @ID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", Selected.Id, UserData.Login, UserData.Password);
                    ShowAnotherTabEvent.Invoke(new Tables.SupplierReturnItemsTable(ShowAnotherTabEvent, OrderItem, Order, ShowMessageEvent, ShowLoginPageEvent));
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
            ShowAnotherTabEvent.Invoke(new Tables.SupplierReturnItemsTable(ShowAnotherTabEvent, OrderItem, Order, ShowMessageEvent, ShowLoginPageEvent));
        }
    }
}
