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
    public partial class CreateCustomerReturnItem: UserControl
    {
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        private CustomerOrderItem OrderItem;
        private CustomerOrder Order;
        public CreateCustomerReturnItem(Events.ShowAnotherTabDelegate ShowAnotherTab, CustomerOrderItem OrderItemObject, CustomerOrder OrderObject)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            OrderItem = OrderItemObject;
            Order = OrderObject;
            DataGrid_Table.ItemsSource = new List<CustomerReturnItem>() { new CustomerReturnItem()};
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            CustomerReturnItem Selected = ((List<CustomerReturnItem>)DataGrid_Table.ItemsSource)[0];
            ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.CreateCustomerReturnItem @OrderItemID = {0},  @Amount = {1},  @Reason = {2}, @AdminLogin = {3}, @AdminPassword = {4}", OrderItem.Id, Selected.Amount, Selected.Reason, UserData.Login, UserData.Password);
            ShowAnotherTabEvent.Invoke(new Tables.CustomerReturnItemsTable(ShowAnotherTabEvent, OrderItem, Order));
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.CustomerReturnItemsTable(ShowAnotherTabEvent, OrderItem, Order));
        }
    }
}