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
        private DispatcherTimer _timer;

        public MainWindow(ServerSession serverSession)
        {
            InitializeComponent();
            if (serverSession != null && serverSession.IsLoggedIn)
            {
                ServerSession = serverSession;
                UsersStatus = new ObservableCollection<ShortUserInformation>(ServerSession.FiliaUsers.GetShortUsersInformation());
                DataGrid.ItemsSource = UsersStatus;
                _timer = new DispatcherTimer();
                _timer.Tick += Timer_Tick;
                _timer.Interval = TimeSpan.FromSeconds(5);
                _timer.Start();
            }
            else
            {
                Close();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var us = ServerSession.FiliaUsers.GetShortUsersInformation();
            foreach (var u in us)
            {
                ShortUserInformation s;
                //Считаем, что пользователь не может быть удалён - только заблокирован
                if ((s = UsersStatus.FirstOrDefault(x => x.Nickname == u.Nickname)) != null)
                {
                    s.CopyProperties(u);
                }
                else
                {
                    UsersStatus.Add(u);
                }
            }
        }

        public ObservableCollection<ShortUserInformation> UsersStatus { get; set; }

        private void getinfo_Click(object sender, RoutedEventArgs e)
        {
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (nick.Text.Length > 0 && nick.Text.Length <= 16 && pswd.Text.Length > 0 && pswd.Text.Length <= 16)
                ServerSession.FiliaUsers.CreateNewUser(nick.Text, pswd.Text, (UserRole) comboBox.SelectedIndex);
            DataGrid.ItemsSource = UsersStatus;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _timer.Stop();
            _timer.Tick -= Timer_Tick;
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
            {
                var w = new Window
                {
                    Content = new UserEditor(ServerSession, ((ShortUserInformation) DataGrid.SelectedItem).Nickname)
                };
                w.Show();
            }
        }
    }
}
