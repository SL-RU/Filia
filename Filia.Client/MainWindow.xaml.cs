using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using Filia.Client.Users;
using Filia.Shared;

namespace Filia.Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ServerSession ServerSession;
        public UsersManager UsersManager;

        public MainWindow(ServerSession serverSession)
        {
            InitializeComponent();
            if (serverSession != null && serverSession.IsLoggedIn)
            {
                ServerSession = serverSession;
                UsersManager = new UsersManager(ServerSession);
                UsersManager.Show();
            }
            else
            {
                Close();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (UsersManager != null)
                UsersManager.Close();
        }

    }
}