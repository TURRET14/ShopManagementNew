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
    public partial class CustomersTable : UserControl
    {
        private List<Customer> CustomersList;

        private event Events.ShowMessageDelegate ShowMessageEvent;
        private event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        private event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public CustomersTable(Events.ShowAnotherTabDelegate ShowAnotherTab, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            try
            {
                CustomersList = ShopManagementContext.GetContext().Customers.FromSqlRaw("EXEC GetCustomers @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                DataGrid_Table.ItemsSource = CustomersList;
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new InsertIntoTables.CreateCustomer(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
        }

        private void DataGrid_Table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGrid_Table.SelectedItem != null)
            {
                ShowAnotherTabEvent.Invoke(new UpdateTables.UpdateCustomer(ShowAnotherTabEvent, (Customer)DataGrid_Table.SelectedItem, ShowMessageEvent, ShowLoginPageEvent));
            }
        }

        private void Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            List<Customer> FilteredList = new List<Customer>(CustomersList);
            if (CheckBox_Name.IsChecked == true)
            {
                if (TextBox_Name.Text.Length > 0)
                {
                    FilteredList = FilteredList.Where(Entry => Entry.Name.ToLower().Contains(TextBox_Name.Text.ToLower())).ToList();
                }
            }
            if (CheckBox_PhoneNumber.IsChecked == true)
            {
                if (TextBox_PhoneNumber.Text.Length > 0)
                {
                    FilteredList = FilteredList.Where(Entry => {
                        if (Entry.PhoneNumber is not null)
                        {
                            return Entry.PhoneNumber.ToLower().Contains(TextBox_PhoneNumber.Text.ToLower());
                        }
                        else
                        {
                            return false;
                        }
                    }).ToList();
                }
            }
            if (CheckBox_Email.IsChecked == true)
            {
                if (TextBox_Email.Text.Length > 0)
                {
                    FilteredList = FilteredList.Where(Entry => {
                        if (Entry.Email is not null)
                        {
                            return Entry.Email.ToLower().Contains(TextBox_Email.Text.ToLower());
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
