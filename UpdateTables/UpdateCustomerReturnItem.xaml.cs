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
    public partial class UpdateCustomerReturnItem : UserControl
    {
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        private CustomerOrderItem OrderItem;
        private CustomerOrder Order;
        public UpdateCustomerReturnItem(Events.ShowAnotherTabDelegate ShowAnotherTab, CustomerReturnItem Selected, CustomerOrderItem OrderItemObject, CustomerOrder OrderObject)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            OrderItem = OrderItemObject;
            Order = OrderObject;
            List<Employee> Employees = ShopManagementContext.GetContext().Employees.FromSqlRaw("EXEC GetEmployeesIDAndNames @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
            Selected.Employee = Employees.FirstOrDefault(Entry => Entry.Id == Selected.EmployeeId);
            DataGrid_Table.ItemsSource = new List<CustomerReturnItem>() { Selected };
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            CustomerReturnItem Selected = ((List<CustomerReturnItem>)DataGrid_Table.ItemsSource)[0];
            ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.DeleteCustomerReturnItem @ID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", Selected.Id, UserData.Login, UserData.Password);
            ShowAnotherTabEvent.Invoke(new Tables.CustomerOrdersTable(ShowAnotherTabEvent));
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.CustomerReturnItemsTable(ShowAnotherTabEvent, OrderItem, Order));
        }
    }
}
