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
    /// Логика взаимодействия для UserCreator.xaml
    /// </summary>
    public partial class UserCreator : Window
    {
        public UserCreator(ServerSession se)
        {
            InitializeComponent();
            ServerSession = se;
        }

        public ServerSession ServerSession { get; set; }

        private void rnd_Click(object sender, RoutedEventArgs e)
        {
            passwordF.Text = Guid.NewGuid().ToString().Substring(0, 7);
        }

        private void canc_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            var a = ServerSession.FiliaUsers.CreateNewUser(nickF.Text, passwordF.Text, (UserRole) roleF.SelectedIndex);
            MessageBox.Show(
                a
                    ? "OK"
                    : "NOPE");
            if(a)
                Close();
        }

        private void nickF_TextChanged(object sender, TextChangedEventArgs e)
        {
            var v = (ServerSession.FiliaUsers.CheckNickname(nickF.Text));
            good.Fill =
                new SolidColorBrush(v
                    ? Color.FromRgb(0, 255, 0)
                    : Color.FromRgb(255, 0, 0));
            add.IsEnabled = v;
        }
    }
}
