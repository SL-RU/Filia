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
    /// Логика взаимодействия для PasswordChanger.xaml
    /// </summary>
    public partial class PasswordChanger : Window
    {
        public PasswordChanger(ServerSession se, string nickname)
        {
            InitializeComponent();
            if (se.NickName == nickname || se.CurrentUser.Role == UserRole.Admin)
            {
                this.nickname.Text = nickname;
                Ok.Click +=
                    (sender, args) =>
                    {
                        MessageBox.Show(se.FiliaUsers.ChangePassword(nickname, passwordBox.Password) ? "OK" : "NOPE");
                        Close();
                    };
            }
            else
            {
                Close();
                MessageBox.Show("NOT ALLOWED!");
            }
            canc.Click += (sender, args) => Close();
        }
    }
}
