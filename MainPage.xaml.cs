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

namespace ShopManagement
{
    public partial class MainPage : UserControl
    {
        public event Events.ShowMessageDelegate ShowMessageEvent;
        public event Events.ShowLoginPageDelegate ShowLoginPageEvent;

        public MainPage(Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            string AccessLevel = "Сотрудник";
            switch (UserData.AccessLevel)
            {
                case "SYSTEM_ADMIN":
                    AccessLevel =  "Системный Администратор";
                    break;
                case "SHOP_ADMIN":
                    AccessLevel = "Администратор";
                    break;
                case "SHOP_MANAGER":
                    AccessLevel = "Менеджер";
                    break;
                case "SHOP_CASHIER":
                    AccessLevel = "Кассир";
                    break;
            }
            Button_UserInfo.Content = AccessLevel + " " + UserData.Login;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            switch (UserData.AccessLevel)
            {
                case "SYSTEM_ADMIN":
                    break;
                case "SHOP_ADMIN":
                    Button_Employees.Visibility = Visibility.Collapsed;
                    break;
                case "SHOP_MANAGER":
                    Button_Employees.Visibility = Visibility.Collapsed;
                    Button_Suppliers.Visibility = Visibility.Collapsed;
                    Button_SupplierOrders.Visibility = Visibility.Collapsed;
                    break;
                case "SHOP_CASHIER":
                    Button_Employees.Visibility = Visibility.Collapsed;
                    Button_Suppliers.Visibility = Visibility.Collapsed;
                    Button_SupplierOrders.Visibility = Visibility.Collapsed;
                    break;
                default:
                    Button_Employees.Visibility = Visibility.Collapsed;
                    Button_Suppliers.Visibility = Visibility.Collapsed;
                    Button_Customers.Visibility = Visibility.Collapsed;
                    Button_Products.Visibility = Visibility.Collapsed;
                    Button_Orders.Visibility = Visibility.Collapsed;
                    Button_SupplierOrders.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void Button_Employees_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTab(new Tables.EmployeesTable(ShowAnotherTab, ShowMessageEvent, ShowLoginPageEvent));
        }

        private void Button_Customers_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTab(new Tables.CustomersTable(ShowAnotherTab, ShowMessageEvent, ShowLoginPageEvent));
        }

        private void Button_Suppliers_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTab(new Tables.SuppliersTable(ShowAnotherTab, ShowMessageEvent, ShowLoginPageEvent));
        }

        private void Button_Products_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTab(new Tables.ProductsTable(ShowAnotherTab, ShowMessageEvent, ShowLoginPageEvent));
        }

        private void Button_Orders_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTab(new Tables.CustomerOrdersTable(ShowAnotherTab, ShowMessageEvent, ShowLoginPageEvent));
        }

        public void ShowAnotherTab(UserControl Tab)
        {
            ContentControl_ForTables.Child = Tab;
        }

        private void TextBlock_UserInfo_Click(object sender, RoutedEventArgs e)
        {
            Popup_UserInfo.IsOpen = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowLoginPageEvent.Invoke();
        }

        private void Button_SupplierOrders_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTab(new Tables.SupplierOrdersTable(ShowAnotherTab, ShowMessageEvent, ShowLoginPageEvent));
        }
    }
}
