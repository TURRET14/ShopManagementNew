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
    public partial class CreateCustomerOrderItem: UserControl
    {
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        private CustomerOrder Order;
        public CreateCustomerOrderItem(Events.ShowAnotherTabDelegate ShowAnotherTab, CustomerOrder OrderObject)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            Order = OrderObject;
            List<Product> Products = ShopManagementContext.GetContext().Products.FromSqlRaw("EXEC GetProducts @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().OrderBy(Entry => Entry.Name).ToList();
            ColumnProduct.ItemsSource = Products;
            DataGrid_Table.ItemsSource = new List<CustomerOrderItem>() { new CustomerOrderItem()};
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            CustomerOrderItem Selected = ((List<CustomerOrderItem>)DataGrid_Table.ItemsSource)[0];
            ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.CreateCustomerOrderItem @OrderID = {0},  @ProductID = {1}, @Amount = {2}, @AdminLogin = {3}, @AdminPassword = {4}", Order.Id, Selected.Product.Id, Selected.Amount, UserData.Login, UserData.Password);
            ShowAnotherTabEvent.Invoke(new Tables.CustomerOrderItemsTable(ShowAnotherTabEvent, Order));
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.CustomerOrderItemsTable(ShowAnotherTabEvent, Order));
        }
    }
}
