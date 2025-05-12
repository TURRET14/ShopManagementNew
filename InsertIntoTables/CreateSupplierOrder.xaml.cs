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
    public partial class CreateSupplierOrder: UserControl
    {
        private event Events.ShowMessageDelegate ShowMessageEvent;
        private event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        private event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        private List<Supplier> SupplierList;
        public CreateSupplierOrder(Events.ShowAnotherTabDelegate ShowAnotherTab, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            try
            {
                SupplierList = ShopManagementContext.GetContext().Suppliers.FromSqlRaw("EXEC GetSuppliers @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().OrderBy(Row => Row.Name).ToList();
                ColumnSupplier.ItemsSource = SupplierList;
                DataGrid_Table.ItemsSource = new List<SupplierOrder>() { new SupplierOrder() };
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
                SupplierOrder Selected = ((List<SupplierOrder>)DataGrid_Table.ItemsSource)[0];
                if (Selected.Supplier is not null)
                {
                    if (Selected.Supplier.Id < 0)
                    {
                        ShowMessageEvent("Ошибка Записи", "Такого поставщика нет!");
                        return;
                    }
                    else if (SupplierList.FirstOrDefault(Selected.Supplier) is null)
                    {
                        ShowMessageEvent("Ошибка Записи", "Такого поставщика нет!");
                        return;
                    }
                }
                else
                {
                    ShowMessageEvent("Ошибка Записи", "Поставщик не может быть пустым!");
                    return;
                }

                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.CreateSupplierOrder @SupplierID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", Selected.Supplier.Id, UserData.Login, UserData.Password);
                ShowAnotherTabEvent.Invoke(new Tables.SupplierOrdersTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.SupplierOrdersTable(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
        }
    }
}
