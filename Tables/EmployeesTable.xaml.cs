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
    public partial class EmployeesTable : UserControl
    {
        private List<Employee> EmployeesList;

        public event Events.ShowMessageDelegate ShowMessageEvent;
        public event Events.ShowLoginPageDelegate ShowLoginPageEvent;
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public EmployeesTable(Events.ShowAnotherTabDelegate ShowAnotherTab, Events.ShowMessageDelegate ShowMessage, Events.ShowLoginPageDelegate ShowLoginPage)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            ShowMessageEvent = ShowMessage;
            ShowLoginPageEvent = ShowLoginPage;
            try
            {
                EmployeesList = ShopManagementContext.GetContext().Employees.FromSqlRaw("EXEC GetEmployees @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
                DataGrid_Table.ItemsSource = EmployeesList;
            }
            catch (SqlException Ex)
            {
                ExceptionHandlers.SqlExceptionHandler(Ex, ShowMessageEvent, ShowLoginPageEvent);
            }

            ComboBox_Gender.ItemsSource = new List<string> { "M", "F" };
            ComboBox_Position.ItemsSource = new List<string>() { "Системный Администратор", "Администратор", "Менеджер", "Кассир" };
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new InsertIntoTables.CreateEmployee(ShowAnotherTabEvent, ShowMessageEvent, ShowLoginPageEvent));
        }

        private void DataGrid_Table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGrid_Table.SelectedItem != null)
            {
                ShowAnotherTabEvent.Invoke(new UpdateTables.UpdateEmployee(ShowAnotherTabEvent, (Employee)DataGrid_Table.SelectedItem, ShowMessageEvent, ShowLoginPageEvent));
            }
        }

        private void Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            List<Employee> FilteredList = new List<Employee>(EmployeesList);
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

            if (CheckBox_Age.IsChecked == true)
            {
                try
                {
                    if (TextBox_AgeBegin.Text.Length > 0)
                    {
                        FilteredList = FilteredList.Where(Entry => Entry.Age >= Convert.ToInt32(TextBox_AgeBegin.Text)).ToList();
                    }
                }
                catch { }
                try
                {
                    if (TextBox_AgeEnd.Text.Length > 0)
                    {
                        FilteredList = FilteredList.Where(Entry => Entry.Age <= Convert.ToInt32(TextBox_AgeEnd.Text)).ToList();
                    }
                }
                catch { }
            }

            if (CheckBox_Gender.IsChecked == true)
            {
                if (ComboBox_Gender.SelectedValue is not null)
                {
                    if (ComboBox_Gender.SelectedValue.ToString().Length > 0)
                    {
                        if (ComboBox_Gender.SelectedValue.ToString() == "M" || ComboBox_Gender.SelectedValue.ToString() == "F")
                        {
                            FilteredList = FilteredList.Where(Entry =>
                            {
                                if (Entry.Gender is not null)
                                {
                                    return Entry.Gender == ComboBox_Gender.SelectedValue.ToString();
                                }
                                else
                                {
                                    return false;
                                }
                            }).ToList();
                        }
                    }
                }
            }

            if (CheckBox_Experience.IsChecked == true)
            {
                try
                {
                    if (TextBox_ExperienceBegin.Text.Length > 0)
                    {
                        FilteredList = FilteredList.Where(Entry => Entry.Experience >= Convert.ToInt32(TextBox_ExperienceBegin.Text)).ToList();
                    }
                }
                catch { }
                try
                {
                    if (TextBox_ExperienceEnd.Text.Length > 0)
                    {
                        FilteredList = FilteredList.Where(Entry => Entry.Experience <= Convert.ToInt32(TextBox_ExperienceEnd.Text)).ToList();
                    }
                }
                catch { }
            }

            if (CheckBox_Position.IsChecked == true)
            {
                if (ComboBox_Position.SelectedValue is not null)
                {
                    if (ComboBox_Position.SelectedValue.ToString().Length > 0)
                    {
                        string SelectedPosition = "";
                        switch (ComboBox_Position.SelectedValue)
                        {
                            case "Системный Администратор":
                                SelectedPosition = "SYSTEM_ADMIN";
                                break;
                            case "Администратор":
                                SelectedPosition = "SHOP_ADMIN";
                                break;
                            case "Менеджер":
                                SelectedPosition = "SHOP_MANAGER";
                                break;
                            case "Кассир":
                                SelectedPosition = "SHOP_CASHIER";
                                break;
                        }
                        if (SelectedPosition == "SYSTEM_ADMIN" || SelectedPosition == "SHOP_ADMIN" || SelectedPosition == "SHOP_MANAGER" || SelectedPosition == "SHOP_CASHIER")
                        {
                            FilteredList = FilteredList.Where(Entry =>
                            {
                                if (Entry.Position is not null)
                                {
                                    return Entry.Position == SelectedPosition;
                                }
                                else
                                {
                                    return false;
                                }
                            }).ToList();
                        }
                    }
                }
            }
            DataGrid_Table.ItemsSource = FilteredList;
        }
    }
}
