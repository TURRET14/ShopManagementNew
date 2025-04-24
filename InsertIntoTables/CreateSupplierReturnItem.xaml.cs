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
    public partial class CreateSupplierReturnItem: UserControl
    {
        public event Events.ShowMessageDelegate ShowMessageEvent;
        public event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        private SupplierOrderItem OrderItem;
        private SupplierOrder Order;
        public CreateSupplierReturnItem(Events.ShowAnotherTabDelegate ShowAnotherTab, SupplierOrderItem OrderItemObject, SupplierOrder OrderObject, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            OrderItem = OrderItemObject;
            Order = OrderObject;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            DataGrid_Table_Info.ItemsSource = new List<SupplierOrderItem>() { OrderItem };
            DataGrid_Table.ItemsSource = new List<SupplierReturnItem>() { new SupplierReturnItem()};
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SupplierReturnItem Selected = ((List<SupplierReturnItem>)DataGrid_Table.ItemsSource)[0];

                if (Selected.Amount < 1)
                {
                    ShowMessageEvent("Ошибка Записи", "Количество Возвращаемого Товара Не Может Быть Меньше 1!");
                    return;
                }

                if (Selected.Reason is not null)
                {
                    if (Selected.Reason.Length > 150)
                    {
                        ShowMessageEvent("Ошибка Записи", "Длина Причины Возврата Не Может Быть Больше 150 Символов!");
                        return;
                    }
                    else if (Selected.Reason.Length == 0)
                    {
                        Selected.Reason = null;
                    }
                }

                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.CreateSupplierReturnItem @OrderItemID = {0},  @Amount = {1},  @Reason = {2}, @AdminLogin = {3}, @AdminPassword = {4}", OrderItem.Id, Selected.Amount, Selected.Reason, UserData.Login, UserData.Password);
                ShowAnotherTabEvent.Invoke(new Tables.SupplierReturnItemsTable(ShowAnotherTabEvent, OrderItem, Order, ShowMessageEvent, ShowLoginPageEvent));
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.SupplierReturnItemsTable(ShowAnotherTabEvent, OrderItem, Order, ShowMessageEvent, ShowLoginPageEvent));
        }
    }
}