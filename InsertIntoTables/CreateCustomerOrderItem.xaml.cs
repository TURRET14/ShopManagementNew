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
    public partial class CreateCustomerOrderItem: UserControl
    {
        private event Events.ShowMessageDelegate ShowMessageEvent;
        private event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        private event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        private CustomerOrder Order;
        private List<Product> ProductList;
        public CreateCustomerOrderItem(Events.ShowAnotherTabDelegate ShowAnotherTab, CustomerOrder OrderObject, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            Order = OrderObject;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            DataGrid_Table_Info.ItemsSource = new List<CustomerOrder>() { Order };
            try
            {
                ProductList = ShopManagementContext.GetContext().Products.FromSqlRaw("EXEC GetProducts @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().OrderBy(Entry => Entry.Name).ToList();
                ColumnProduct.ItemsSource = ProductList;
                DataGrid_Table.ItemsSource = new List<CustomerOrderItem>() { new CustomerOrderItem() };
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
                CustomerOrderItem Selected = ((List<CustomerOrderItem>)DataGrid_Table.ItemsSource)[0];

                if (Selected.Product is not null)
                {
                    if (Selected.Product.Id < 0)
                    {
                        ShowMessageEvent("Ошибка Записи", "Такого товара нет!");
                        return;
                    }
                    else if (ProductList.FirstOrDefault(Selected.Product) is null)
                    {
                        ShowMessageEvent("Ошибка Записи", "Такого товара нет!");
                        return;
                    }
                }
                else
                {
                    ShowMessageEvent("Ошибка Записи", "Товар не может быть пустым!");
                    return;
                }

                if (Selected.Amount < 1)
                {
                    ShowMessageEvent("Ошибка Записи", "Количество товара не может быть меньше 1!");
                    return;
                }

                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.CreateCustomerOrderItem @OrderID = {0},  @ProductID = {1}, @Amount = {2}, @AdminLogin = {3}, @AdminPassword = {4}", Order.Id, Selected.Product.Id, Selected.Amount, UserData.Login, UserData.Password);
                ShowAnotherTabEvent.Invoke(new Tables.CustomerOrderItemsTable(ShowAnotherTabEvent, Order, ShowMessageEvent, ShowLoginPageEvent));
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.CustomerOrderItemsTable(ShowAnotherTabEvent, Order, ShowMessageEvent, ShowLoginPageEvent));
        }
    }
}
