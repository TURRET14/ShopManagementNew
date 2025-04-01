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

namespace ShopManagement.UpdateTables
{
    public partial class UpdateEmployee : UserControl
    {
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public UpdateEmployee(Events.ShowAnotherTabDelegate ShowAnotherTab, Employee Selected)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            ColumnGender.ItemsSource = new List<string>() { "M", "F"};
            ColumnPosition.ItemsSource = new List<string>() { "SYSTEM_ADMIN", "SHOP_ADMIN", "SHOP_MANAGER", "SHOP_CASHIER"};
            DataGrid_Table.ItemsSource = new List<Employee> { Selected };
        }

        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            Employee Selected = ((List<Employee>)DataGrid_Table.ItemsSource)[0];
            ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.UpdateEmployee @ID = {0}, @Name = {1},  @Age = {2}, @Gender = {3}, @PhoneNumber = {4}, @Email = {5}, @Experience = {6}, @Position = {7}, @UserLogin = {8}, @UserPassword = {9}, @AdminLogin = {10}, @AdminPassword = {11}", Selected.Id, Selected.Name, Selected.Age, Selected.Gender, Selected.PhoneNumber, Selected.Email, Selected.Experience, Selected.Position, Selected.UserLogin, Selected.UserPassword, UserData.Login, UserData.Password);
            ShowAnotherTabEvent.Invoke(new Tables.EmployeesTable(ShowAnotherTabEvent));
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            Employee Selected = ((List<Employee>)DataGrid_Table.ItemsSource)[0];
            ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.DeleteEmployee @ID = {0}, @AdminLogin = {1}, @AdminPassword = {2}", Selected.Id, UserData.Login, UserData.Password);
            ShowAnotherTabEvent.Invoke(new Tables.EmployeesTable(ShowAnotherTabEvent));
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.EmployeesTable(ShowAnotherTabEvent));
        }
    }
}
