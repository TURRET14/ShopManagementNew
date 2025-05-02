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

namespace ShopManagement.Tables
{
    public partial class SupplierOrderItemsTable : UserControl
    {
        private List<SupplierOrderItem> OrderItemsList;

        private event Events.ShowMessageDelegate ShowMessageEvent;
        private event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        private event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        private SupplierOrder Order;
        public SupplierOrderItemsTable(Events.ShowAnotherTabDelegate ShowAnotherTab, SupplierOrder OrderObject, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            Order = OrderObject;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            DataGrid_Header.ItemsSource = new List<SupplierOrder> { Order };
            try
            {
                OrderItemsList = ShopManagementContext.GetContext().SupplierOrderItems.FromSqlRaw("EXEC GetSupplierOrderItems @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().Where(Entry => Entry.OrderId == Order.Id).ToList();
                List<Product> Products = ShopManagementContext.GetContext().Products.FromSqlRaw("EXEC GetProducts @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                foreach (SupplierOrderItem OrderItem in OrderItemsList)
                {
                    OrderItem.Product = Products.FirstOrDefault(Entry => Entry.Id == OrderItem.ProductId);
                }
                DataGrid_Table.ItemsSource = OrderItemsList;
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new InsertIntoTables.CreateSupplierOrderItem(ShowAnotherTabEvent, Order, ShowMessageEvent, ShowLoginPageEvent));
        }

        private void DataGrid_Table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGrid_Table.SelectedItem != null)
            {
                ShowAnotherTabEvent.Invoke(new Tables.SupplierReturnItemsTable(ShowAnotherTabEvent, (SupplierOrderItem)DataGrid_Table.SelectedItem, Order, ShowMessageEvent, ShowLoginPageEvent));
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
                    ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.DeleteSupplierOrder @ID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", Order.Id, UserData.Login, UserData.Password);
                    ShowAnotherTabEvent.Invoke(new Tables.SupplierOrdersTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
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
            ShowAnotherTabEvent.Invoke(new Tables.SupplierOrdersTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
        }

        private void Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            List<SupplierOrderItem> FilteredList = new List<SupplierOrderItem>(OrderItemsList);
            if (CheckBox_ProductName.IsChecked == true)
            {
                if (TextBox_ProductName.Text.Length > 0)
                {
                    FilteredList = FilteredList.Where(Entry => {
                        if (Entry.Product is not null)
                        {
                            return Entry.Product.Name.ToLower().Contains(TextBox_ProductName.Text.ToLower());
                        }
                        else
                        {
                            return false;
                        }
                    }).ToList();
                }
            }
            if (CheckBox_Price.IsChecked == true)
            {
                try
                {
                    if (TextBox_PriceBegin.Text.Length > 0)
                    {
                        FilteredList = FilteredList.Where(Entry => Entry.Price >= Convert.ToDecimal(TextBox_PriceBegin.Text)).ToList();
                    }
                }
                catch { }
                try
                {
                    if (TextBox_PriceEnd.Text.Length > 0)
                    {
                        FilteredList = FilteredList.Where(Entry => Entry.Price <= Convert.ToDecimal(TextBox_PriceEnd.Text)).ToList();
                    }
                }
                catch { }
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
            DataGrid_Table.ItemsSource = FilteredList;
        }
    }
}
