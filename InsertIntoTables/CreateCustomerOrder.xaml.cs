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
        private event Events.ShowMessageDelegate ShowMessageEvent;
        private event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        private event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        private List<Customer> CustomerList;
        public CreateCustomerOrder(Events.ShowAnotherTabDelegate ShowAnotherTab, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            try
            {
                CustomerList = ShopManagementContext.GetContext().Customers.FromSqlRaw("EXEC GetCustomers @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().OrderBy(Row => Row.Name).ToList();
                ColumnCustomer.ItemsSource = CustomerList;
                DataGrid_Table.ItemsSource = new List<CustomerOrder>() { new CustomerOrder() };
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomerOrder Selected = ((List<CustomerOrder>)DataGrid_Table.ItemsSource)[0];
                if (Selected.Customer is not null)
                {
                    if (Selected.Customer.Id < 0)
                    {
                        ShowMessageEvent("Ошибка Записи", "Такого Клиента Нет!");
                        return;
                    }
                    else if (CustomerList.FirstOrDefault(Selected.Customer) is null)
                    {
                        ShowMessageEvent("Ошибка Записи", "Такого Клиента Нет!");
                        return;
                    }
                }
                else
                {
                    ShowMessageEvent("Ошибка Записи", "Клиент Не Может Быть Пустым!");
                    return;
                }

                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.CreateCustomerOrder @CustomerID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", Selected.Customer.Id, UserData.Login, UserData.Password);
                ShowAnotherTabEvent.Invoke(new Tables.CustomerOrdersTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.CustomerOrdersTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
        }
    }
}
