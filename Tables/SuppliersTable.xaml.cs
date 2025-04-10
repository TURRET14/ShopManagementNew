using ShopManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace ShopManagement.Tables
{
    public partial class SuppliersTable : UserControl
    {
        private List<Supplier> SuppliersList;
        public event Events.ShowMessageDelegate ShowMessageEvent;
        public event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public SuppliersTable(Events.ShowAnotherTabDelegate ShowAnotherTab, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            try
            {
                SuppliersList = ShopManagementContext.GetContext().Suppliers.FromSqlRaw("EXEC GetSuppliers @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                DataGrid_Table.ItemsSource = SuppliersList;
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new InsertIntoTables.CreateSupplier(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
        }

        private void DataGrid_Table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGrid_Table.SelectedItem != null)
            {
                ShowAnotherTabEvent.Invoke(new UpdateTables.UpdateSupplier(ShowAnotherTabEvent, (Supplier)DataGrid_Table.SelectedItem, ShowMessageEvent, ShowLoginPageEvent));
            }
        }

        private void Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            List<Supplier> FilteredList = new List<Supplier>(SuppliersList);
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
            if (CheckBox_AccountNumber.IsChecked == true)
            {
                if (TextBox_AccountNumber.Text.Length > 0)
                {
                    FilteredList = FilteredList.Where(Entry => {
                        if (Entry.AccountNumber is not null)
                        {
                            return Entry.AccountNumber.ToLower().Contains(TextBox_AccountNumber.Text.ToLower());
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
