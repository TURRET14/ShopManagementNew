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

namespace ShopManagement.Tables
{
    public partial class CustomerOrderItemsTable : UserControl
    {
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        private CustomerOrder Order;
        public CustomerOrderItemsTable(Events.ShowAnotherTabDelegate ShowAnotherTab, CustomerOrder OrderObject)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            Order = OrderObject;
            DataGrid_Header.ItemsSource = new List<CustomerOrder> { Order };
            List<CustomerOrderItem> OrderItems = ShopManagementContext.GetContext().CustomerOrderItems.FromSqlRaw("EXEC GetCustomerOrderItems @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().Where(Entry => Entry.OrderId == Order.Id).ToList();
            List<Product> Products = ShopManagementContext.GetContext().Products.FromSqlRaw("EXEC GetProducts @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
            foreach (CustomerOrderItem OrderItem in OrderItems)
            {
                OrderItem.Product = Products.FirstOrDefault(Entry => Entry.Id == OrderItem.ProductId);
            }
            DataGrid_Table.ItemsSource = OrderItems;
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new InsertIntoTables.CreateCustomerOrderItem(ShowAnotherTabEvent, Order));
        }

        private void DataGrid_Table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGrid_Table.SelectedItem != null)
            {
                ShowAnotherTabEvent.Invoke(new Tables.CustomerReturnItemsTable(ShowAnotherTabEvent, (CustomerOrderItem)DataGrid_Table.SelectedItem, Order));
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.DeleteCustomerOrder @ID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", Order.Id, UserData.Login, UserData.Password);
            ShowAnotherTabEvent.Invoke(new Tables.CustomerOrdersTable(ShowAnotherTabEvent));
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.CustomerOrdersTable(ShowAnotherTabEvent));
        }
    }
}
