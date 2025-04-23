using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ShopManagement
{
    public partial class Dictionary : ResourceDictionary
    {
        private void DataGrid_LostFocus(object sender, RoutedEventArgs e)
        {
            ((DataGrid)sender).SelectedItem = null;
        }
    }
}
