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
    public partial class EmployeesTable : UserControl
    {
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public EmployeesTable(Events.ShowAnotherTabDelegate ShowAnotherTab)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            DataGrid_Table.ItemsSource = ShopManagementContext.GetContext().Employees.FromSqlRaw("EXEC GetEmployees @AdminLogin = {0}, @AdminPassword = {1}", UserData.Login, UserData.Password).AsNoTracking().AsEnumerable().ToList();
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new InsertIntoTables.CreateEmployee(ShowAnotherTabEvent));
        }

        private void DataGrid_Table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGrid_Table.SelectedItem != null)
            {
                ShowAnotherTabEvent.Invoke(new UpdateTables.UpdateEmployee(ShowAnotherTabEvent, (Employee)DataGrid_Table.SelectedItem));
            }
        }
    }
}
