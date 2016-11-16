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
using Filia.Shared;

namespace Filia.Client.Users
{
    /// <summary>
    /// Логика взаимодействия для UserEditor.xaml
    /// </summary>
    public partial class UserEditor : UserControl
    {
        public UserEditor(ServerSession serverSession)
        {
            ServerSession = serverSession;
            InitializeComponent();
        }

        public UserEditor(ServerSession serverSession, string nickname)
        {
            ServerSession = serverSession;
            InitializeComponent();
            EditUser(nickname);
            
        }

        public ServerSession ServerSession;
        public UserInformation UserInformation { get; private set; }

        public void EditUser(string nickname)
        {
            var u = ServerSession.FiliaUsers.GetUserInformation(nickname);
            EditUser(u);
        }

        public void EditUser(UserInformation ui)
        {
            UserInformation = ui;
            DataContext = ui;
            roleF.SelectedIndex = (int) ui.Role;
            if (ServerSession.CurrentUser.Role != UserRole.Admin && ServerSession.NickName != ui.Nickname)
            {
                IsEnabled = false;
            }
            else
            {
                IsEnabled = true;
            }
        }

        private void passwordB_Click(object sender, RoutedEventArgs e)
        {
            new PasswordChanger(ServerSession, UserInformation.Nickname).Show();
        }

        private void cancelB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ((Window) Parent).Close();
            }
            catch (Exception)
            {
                // ignored
            }
        }


        private void okB_Click(object sender, RoutedEventArgs e)
        {
            UserInformation u = new UserInformation(UserInformation.Id, UserInformation.Nickname, realnameF.Text,
                aboutF.Text, emailF.Text, (UserRole) roleF.SelectedIndex, uploadF.IsChecked.Value, false);
            if (ServerSession.FiliaUsers.SetUserInformation(u))
                MessageBox.Show("OK");
            else
            {
                MessageBox.Show("NOPE");
            }
            EditUser(u.Nickname);
        }
    }
}