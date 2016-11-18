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
using Filia.Shared;

namespace Filia.Client.Users
{
    /// <summary>
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class UsersManager : Window
    {
        public UsersManager(ServerSession se)
        {
            InitializeComponent();
            ServerSession = se;
            UsersList.SetServerSession(se);
            UserEditor = new UserEditor(ServerSession);
            uegrid.Children.Add(UserEditor);
            UsersList.OnUserSelected += nickname => UserEditor.EditUser(nickname);
        }

        public ServerSession ServerSession { get; private set; }
        public UserEditor UserEditor { get; private set; }

        private void editinfo_Click(object sender, RoutedEventArgs e)
        {
            UserEditor.EditUser(ServerSession.NickName);
        }

        private void newuser_Click(object sender, RoutedEventArgs e)
        {
            var v = new UserCreator(ServerSession);
            v.Show();
            v.Closing += (o, args) => UsersList.LoadUsers();
        }

        private void changepassword_Click(object sender, RoutedEventArgs e)
        {
            var  v = new PasswordChanger(ServerSession, ServerSession.NickName);
            v.Show();
            v.Closing += (o, args) => UsersList.LoadUsers();
        }
    }
}
