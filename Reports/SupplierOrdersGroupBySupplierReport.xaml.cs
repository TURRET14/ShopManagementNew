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

namespace ShopManagement.Reports
{
    public partial class SupplierOrdersGroupBySupplierReport : UserControl
    {
        public event Events.ShowMessageDelegate ShowMessageEvent;
        public event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        public SupplierOrdersGroupBySupplierReport(Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            try
            {
                DataGrid_Table.ItemsSource = ShopManagementContext.GetContext().SupplierOrdersGroupBySuppliers.FromSqlRaw("EXEC GetSupplierOrdersGroupBySupplier @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_Date.IsChecked == true)
            {
                if (DatePicker_DateBegin.SelectedDate is not null || DatePicker_DateEnd.SelectedDate is not null)
                {
                    DateTimeOffset? DateBegin;
                    DateTimeOffset? DateEnd;
                    if (DatePicker_DateBegin.SelectedDate is not null)
                    {
                        DateBegin = new DateTimeOffset(DatePicker_DateBegin.SelectedDate.Value);
                    }
                    else
                    {
                        DateBegin = null;
                    }
                    if (DatePicker_DateEnd.SelectedDate is not null)
                    {
                        DateEnd = new DateTimeOffset(DatePicker_DateEnd.SelectedDate.Value).AddDays(1);
                    }
                    else
                    {
                        DateEnd = null;
                    }

                    try
                    {
                        DataGrid_Table.ItemsSource = ShopManagementContext.GetContext().SupplierOrdersGroupBySuppliers.FromSqlRaw("EXEC GetSupplierOrdersGroupBySupplier @DateBegin = {0}, @DateEnd = {1}, @AdminLogin = {2}, @AdminPassword = {3}", DateBegin.GetValueOrDefault(new DateTimeOffset(1, 1, 1, 0, 0, 0, new TimeSpan())), DateEnd.GetValueOrDefault(new DateTimeOffset(9999, 1, 1, 0, 0, 0, new TimeSpan())), UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                    }
                    catch (SqlException Ex)
                    {
                        ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
                    }
                }
                else
                {
                    try
                    {
                        DataGrid_Table.ItemsSource = ShopManagementContext.GetContext().SupplierOrdersGroupBySuppliers.FromSqlRaw("EXEC GetSupplierOrdersGroupBySupplier @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                    }
                    catch (SqlException Ex)
                    {
                        ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
                    }
                }
            }
            else
            {
                try
                {
                    DataGrid_Table.ItemsSource = ShopManagementContext.GetContext().SupplierOrdersGroupBySuppliers.FromSqlRaw("EXEC GetSupplierOrdersGroupBySupplier @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                }
                catch (SqlException Ex)
                {
                    ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
                }
            }
        }
    }
}
