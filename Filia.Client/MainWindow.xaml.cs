using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                textBlock.Text += "[RET] " + ServerSession.FiliaProxy.GetAllData(new Action<string>(s =>
                {
                    textBlock.Dispatcher.BeginInvoke(new Action(delegate()
                    {
                        textBlock.Text += "[DATA] " + s + "\n";
                    }));
                })) + "\n";
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
            ServerSession.FiliaProxy.SendMessage(textBox.Text);
        }

        private void getinfo_Click(object sender, RoutedEventArgs e)
        {
             var v = ServerSession.FiliaProxy.GetUserInformation(textBox.Text);
            MessageBox.Show(string.Format("id: {0}\nnick: {1}\nname: {2}\nrole: {3}\nimg {4}", v.Id, v.Nickname, v.Realname, v.Role, v.UploadImages), "v",
                MessageBoxButton.OK);
        }
    }
}
