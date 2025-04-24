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
    public partial class CreateSupplierOrderItem: UserControl
    {
        public event Events.ShowMessageDelegate ShowMessageEvent;
        public event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        private SupplierOrder Order;
        private List<Product> ProductList;
        public CreateSupplierOrderItem(Events.ShowAnotherTabDelegate ShowAnotherTab, SupplierOrder OrderObject, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            Order = OrderObject;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            DataGrid_Table_Info.ItemsSource = new List<SupplierOrder>() { Order };
            try
            {
                ProductList = ShopManagementContext.GetContext().Products.FromSqlRaw("EXEC GetProducts @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().OrderBy(Entry => Entry.Name).ToList();
                ColumnProduct.ItemsSource = ProductList;
                DataGrid_Table.ItemsSource = new List<SupplierOrderItem>() { new SupplierOrderItem() };
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
                SupplierOrderItem Selected = ((List<SupplierOrderItem>)DataGrid_Table.ItemsSource)[0];

                if (Selected.Product is not null)
                {
                    if (Selected.Product.Id < 0)
                    {
                        ShowMessageEvent("Ошибка Записи", "Такого Товара Нет!");
                        return;
                    }
                    else if (ProductList.FirstOrDefault(Selected.Product) is null)
                    {
                        ShowMessageEvent("Ошибка Записи", "Такого Товара Нет!");
                        return;
                    }
                }
                else
                {
                    ShowMessageEvent("Ошибка Записи", "Товар Не Может Быть Пустым!");
                    return;
                }

                if (Selected.Amount < 1)
                {
                    ShowMessageEvent("Ошибка Записи", "Количество Товара Не Может Быть Меньше 1!");
                    return;
                }

                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.CreateSupplierOrderItem @OrderID = {0},  @ProductID = {1}, @Amount = {2}, @Price = {3}, @AdminLogin = {4}, @AdminPassword = {5}", Order.Id, Selected.Product.Id, Selected.Amount, Selected.Price, UserData.Login, UserData.Password);
                ShowAnotherTabEvent.Invoke(new Tables.SupplierOrderItemsTable(ShowAnotherTabEvent, Order, ShowMessageEvent, ShowLoginPageEvent));
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.SupplierOrderItemsTable(ShowAnotherTabEvent, Order, ShowMessageEvent, ShowLoginPageEvent));
        }
    }
}
