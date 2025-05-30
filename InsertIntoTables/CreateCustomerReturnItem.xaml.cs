﻿using Microsoft.Data.SqlClient;
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
    public partial class CreateCustomerReturnItem: UserControl
    {
        private event Events.ShowMessageDelegate ShowMessageEvent;
        private event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        private event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        private CustomerOrderItem OrderItem;
        private CustomerOrder Order;
        public CreateCustomerReturnItem(Events.ShowAnotherTabDelegate ShowAnotherTab, CustomerOrderItem OrderItemObject, CustomerOrder OrderObject, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            OrderItem = OrderItemObject;
            Order = OrderObject;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            DataGrid_Table_Info.ItemsSource = new List<CustomerOrderItem>() { OrderItem };
            DataGrid_Table.ItemsSource = new List<CustomerReturnItem>() { new CustomerReturnItem()};
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomerReturnItem Selected = ((List<CustomerReturnItem>)DataGrid_Table.ItemsSource)[0];

                if (Selected.Amount < 1)
                {
                    ShowMessageEvent("Ошибка Записи", "Количество возвращаемого товара не может быть меньше 1!");
                    return;
                }

                if (Selected.Reason is not null)
                {
                    if (Selected.Reason.Length > 150)
                    {
                        ShowMessageEvent("Ошибка Записи", "Длина причины возврата не может быть больше 150 символов!");
                        return;
                    }
                    else if (Selected.Reason.Length == 0)
                    {
                        Selected.Reason = null;
                    }
                }

                ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.CreateCustomerReturnItem @OrderItemID = {0},  @Amount = {1},  @Reason = {2}, @AdminLogin = {3}, @AdminPassword = {4}", OrderItem.Id, Selected.Amount, Selected.Reason, UserData.Login, UserData.Password);
                ShowAnotherTabEvent.Invoke(new Tables.CustomerReturnItemsTable(ShowAnotherTabEvent, OrderItem, Order, ShowMessageEvent, ShowLoginPageEvent));
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.CustomerReturnItemsTable(ShowAnotherTabEvent, OrderItem, Order, ShowMessageEvent, ShowLoginPageEvent));
        }
    }
}