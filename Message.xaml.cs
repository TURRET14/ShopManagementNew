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
    
    public partial class Message : UserControl
    {
        public delegate void HideMessageDelegate(Message Msg);
        public event HideMessageDelegate HideMessageEvent;
        public Message(string HeaderText, string MessageText, HideMessageDelegate HideEvent)
        {
            InitializeComponent();
            HideMessageEvent = HideEvent;
            TextBlock_DialogHeader.Text = HeaderText;
            TextBlock_DialogText.Text = MessageText;
        }

        private void Button_Ok_Click(object sender, RoutedEventArgs e)
        {
            HideMessageEvent.Invoke(this);
        }
    }
}
