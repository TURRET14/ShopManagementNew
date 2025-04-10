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

namespace ShopManagement.Tables
{
    public partial class ProductsTable : UserControl
    {
        private List<Product> ProductsList;
        public event Events.ShowMessageDelegate ShowMessageEvent;
        public event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public ProductsTable(Events.ShowAnotherTabDelegate ShowAnotherTab, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            try
            {
                ProductsList = ShopManagementContext.GetContext().Products.FromSqlRaw("EXEC GetProducts @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                DataGrid_Table.ItemsSource = ProductsList;
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new InsertIntoTables.CreateProduct(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
        }

        private void DataGrid_Table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGrid_Table.SelectedItem != null)
            {
                ShowAnotherTabEvent.Invoke(new UpdateTables.UpdateProduct(ShowAnotherTabEvent, (Product)DataGrid_Table.SelectedItem, ShowMessageEvent, ShowLoginPageEvent));
            }
        }

        private void Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            List<Product> FilteredList = new List<Product>(ProductsList);
            if (CheckBox_Name.IsChecked == true)
            {
                if (TextBox_Name.Text.Length > 0)
                {
                    FilteredList = FilteredList.Where(Entry => Entry.Name.ToLower().Contains(TextBox_Name.Text.ToLower())).ToList();
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
            if (CheckBox_Description.IsChecked == true)
            {
                if (TextBox_Description.Text.Length > 0)
                {
                    FilteredList = FilteredList.Where(Entry => {
                        if (Entry.Description is not null)
                        {
                            return Entry.Description.ToLower().Contains(TextBox_Description.Text.ToLower());
                        }
                        else
                        {
                            return false;
                        }
                    }).ToList();
                }
            }
            DataGrid_Table.ItemsSource = FilteredList;
        }
    }
}
