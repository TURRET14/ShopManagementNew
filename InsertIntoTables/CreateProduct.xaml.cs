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
    public partial class CreateProduct : UserControl
    {
        public event Events.ShowAnotherTabDelegate ShowAnotherTabEvent;
        public CreateProduct(Events.ShowAnotherTabDelegate ShowAnotherTab)
        {
            InitializeComponent();
            ShowAnotherTabEvent = ShowAnotherTab;
            DataGrid_Table.ItemsSource = new List<Product>() { new Product()};
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)
        {
            Product Selected = ((List<Product>)DataGrid_Table.ItemsSource)[0];
            ShopManagementContext.GetContext().Database.ExecuteSqlRaw("EXEC Dbo.CreateProduct @Name = {0},  @Price = {1}, @Amount = {2}, @Description = {3}, @AdminLogin = {4}, @AdminPassword = {5}", Selected.Name, Selected.Price, Selected.Amount, Selected.Description, UserData.Login, UserData.Password);
            ShowAnotherTabEvent.Invoke(new Tables.ProductsTable(ShowAnotherTabEvent));
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            ShowAnotherTabEvent.Invoke(new Tables.ProductsTable(ShowAnotherTabEvent));
        }
    }
}
