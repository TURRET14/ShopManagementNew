using System.Text;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowLoginPage();
            //VisualTreeHelper.SetRootDpi(this, new DpiScale(5, 5));
            //this.DpiChanged += (OldDPI, NewDPI) => { InvalidateVisual(); InvalidateMeasure(); InvalidateArrange(); };
        }

        public void ShowMessage(string Header, string Text)
        {
            MainGrid.Children.Add(new Message(Header, Text, HideMessage));
        }

        public void HideMessage(Message Msg)
        {
            MainGrid.Children.Remove(Msg);
        }

        public void ShowMainPage()
        {
            MainGrid.Children.Clear();
            MainGrid.Children.Add(new MainPage(ShowMessage, ShowLoginPage));
        }
        
        public void ShowLoginPage()
        {
            MainGrid.Children.Add(new LoginPage(ShowMessage, ShowMainPage));
        }
    }
}