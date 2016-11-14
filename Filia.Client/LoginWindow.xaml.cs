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
using System.Windows.Shapes;

namespace Filia.Client
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(ServerSession serverSession = null)
        {
            InitializeComponent();
            if (serverSession != null && serverSession.IsLoggedIn)
            {
                ServerSession = serverSession;
                ServerBox.Text = ServerSession.ServerAdress;
                LoginBox.Text = ServerSession.NickName;
                LoginButton.IsEnabled = false;
                LogoutButton.IsEnabled = true;
            }
            else
            {
                ServerSession = null;
                LoginButton.IsEnabled = true;
                LogoutButton.IsEnabled = false;
            }
            LoginBox.Focus();
        }

        public LoginWindow()
        {
            InitializeComponent();

            ServerSession = null;
            LoginButton.IsEnabled = true;
            LogoutButton.IsEnabled = false;
        }


        public ServerSession ServerSession;

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServerSession == null)
            {
                ServerSession = new ServerSession();
            }

            if (ServerSession.Login(LoginBox.Text, ServerBox.Text, PasswordBox.Password))
            {
                LoginButton.IsEnabled = false;
                LogoutButton.IsEnabled = true;
                MessageBox.Show("Logged in!");
                MainWindow mw = new MainWindow(ServerSession);
                mw.Show();
            }
            else
            {
                ServerSession.Logout();
                LoginButton.IsEnabled = true;
                LogoutButton.IsEnabled = false;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServerSession != null)
            {
                ServerSession.Logout();
            }
            LoginButton.IsEnabled = true;
            LogoutButton.IsEnabled = false;
        }

        private void ServerBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox s = e.Source as TextBox;
                if (s != null)
                {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }

                e.Handled = true;
            }
        }

        private void PasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PasswordBox s = e.Source as PasswordBox;
                if (s != null)
                {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }

                e.Handled = true;
            }
        }
    }
}