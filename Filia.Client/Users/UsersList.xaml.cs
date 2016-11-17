using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using Filia.Shared;

namespace Filia.Client.Users
{
    /// <summary>
    /// Логика взаимодействия для UsersList.xaml
    /// </summary>
    public partial class UsersList : UserControl
    {
        public UsersList()
        {
            InitializeComponent();
        }

        public ServerSession ServerSession { get; private set; }
        public ObservableCollection<ShortUserInformation> UsersStatus { get; set; }
        private DispatcherTimer _timer;

        public void SetServerSession(ServerSession ss)
        {
            ServerSession = ss;
            LoadUsers();
        }

        public void LoadUsers()
        {
            if (UsersStatus != null)
            {
                Timer_Tick(null, null);
                return;
            }
            UsersStatus = new ObservableCollection<ShortUserInformation>();
            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;
            _timer.Interval = TimeSpan.FromSeconds(5);
            _timer.Start();

            DataGrid.ItemsSource = UsersStatus;
            Timer_Tick(null, null);
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

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGrid.SelectedItem == null) return;
            var ur = new UserEditor(ServerSession, ((ShortUserInformation) DataGrid.SelectedItem).Nickname);
            var w = new Window
            {
                Content = ur,
                MinHeight = ur.MinHeight+35,
                MinWidth = ur.MinWidth+40,
                Width = ur.MinWidth,
            };
            w.PreviewKeyDown += (o, args) => { if (args.Key == Key.Escape) w.Close(); };
            w.Show();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _timer.Tick -= Timer_Tick;
        }
    }

    public class RoleToUserStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return ((UserRole)value).ToString()[0].ToString();
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
