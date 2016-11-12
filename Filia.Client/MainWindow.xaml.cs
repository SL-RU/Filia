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

namespace Filia.Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ServerSession ServerSession;
        public MainWindow(ServerSession serverSession)
        {
            InitializeComponent();
            if (serverSession != null && serverSession.IsLoggedIn)
            {
                ServerSession = serverSession;
                ServerSession.FiliaProxy.MessageReceived += new Action<string, string>(MessageReceived);
            }
            else
            {
                Close();
            }
        }

        private void MessageReceived(string s, string s1)
        {
            textBlock.Dispatcher.BeginInvoke(new Action(delegate()
            {
                textBlock.Text += "[" + s + "] " + s1 + "\n";
            }));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ServerSession.FiliaProxy.SendMessage(ServerSession.NickName, textBox.Text);
        }
    }
}
