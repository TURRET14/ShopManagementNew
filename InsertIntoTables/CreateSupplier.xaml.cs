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
    public partial class CreateSupplier: UserControl
    {
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public CreateSupplier(Events.ShowAnotherTabDelegate ShowAnotherTab)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            DataGrid_Table.ItemsSource = new List<Supplier>() { new Supplier()};
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            Supplier Selected = ((List<Supplier>)DataGrid_Table.ItemsSource)[0];
            ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.CreateSupplier @Name = {0},  @PhoneNumber = {1}, @Email = {2}, @AccountNumber = {3}, @AdminLogin = {4}, @AdminPassword = {5}", Selected.Name, Selected.PhoneNumber, Selected.Email, Selected.AccountNumber, UserData.Login, UserData.Password);
            ShowAnotherTabEvent.Invoke(new Tables.SuppliersTable(ShowAnotherTabEvent));
        }
        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.SuppliersTable(ShowAnotherTabEvent));
        }
    }
}
