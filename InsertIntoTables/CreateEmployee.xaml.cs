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
    public partial class CreateEmployee : UserControl
    {
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public CreateEmployee(Events.ShowAnotherTabDelegate ShowAnotherTab)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            ColumnGender.ItemsSource = new List<string>() { "M", "F" };
            ColumnPosition.ItemsSource = new List<string>() { "SHOP_ADMIN", "SHOP_MANAGER", "SHOP_CASHIER" };
            DataGrid_Table.ItemsSource = new List<Employee>() { new Employee()};
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            Employee Selected = ((List<Employee>)DataGrid_Table.ItemsSource)[0];
            ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.CreateEmployee @Name = {0},  @Age = {1}, @Gender = {2}, @PhoneNumber = {3}, @Email = {4}, @Experience = {5}, @Position = {6}, @UserLogin = {7}, @UserPassword = {8}, @AdminLogin = {9}, @AdminPassword = {10}", Selected.Name, Selected.Age, Selected.Gender, Selected.PhoneNumber, Selected.Email, Selected.Experience, Selected.Position, Selected.UserLogin, Selected.UserPassword, UserData.Login, UserData.Password);
            ShowAnotherTabEvent.Invoke(new Tables.EmployeesTable(ShowAnotherTabEvent));
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.EmployeesTable(ShowAnotherTabEvent));
        }
    }
}
