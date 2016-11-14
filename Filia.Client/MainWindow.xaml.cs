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
using Filia.Shared;

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
                UsersStatus = new Dictionary<string, bool>(ServerSession.FiliaProxy.GetUsersOnlineStatus());
                DataGrid.ItemsSource = UsersStatus;
            }
            else
            {
                Close();
            }
        }

        public Dictionary<string, bool> UsersStatus { get; set; }

        private void getinfo_Click(object sender, RoutedEventArgs e)
        {
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (nick.Text.Length > 0 && nick.Text.Length <= 16 && pswd.Text.Length > 0 && pswd.Text.Length <= 16)
                ServerSession.FiliaProxy.CreateNewUser(nick.Text, pswd.Text, (UserRole) comboBox.SelectedIndex);
            UsersStatus = new Dictionary<string, bool>(ServerSession.FiliaProxy.GetUsersOnlineStatus());
            DataGrid.ItemsSource = UsersStatus;
        }
    }
}
