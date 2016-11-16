using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using Filia.Server;
using Filia.Shared;
using Zyan.Communication;
using Zyan.Communication.Protocols.Tcp;

namespace Filia.Client
{
    public class ServerSession
    {
        public ServerSession()
        {
            _protocol = new TcpDuplexClientProtocolSetup(true);
            _timer = new Timer(1000);
            _timer.Elapsed += TimerOnElapsed;
        }


        public bool IsLoggedIn => _isLoggedIn && _connection != null && _connection.IsSessionValid;
        public string ServerAdress { get; private set; } = "";
        public string NickName { get; private set; } = "";
        public IFilia Filia { get; private set; }
        public IFiliaUsers FiliaUsers { get; private set; }
        public ShortUserInformation CurrentUser { get; private set; }

        private readonly Timer _timer;
        private ZyanConnection _connection = null;
        private readonly TcpDuplexClientProtocolSetup _protocol;
        private bool _isLoggedIn;

        public bool Login(string login, string server, string password)
        {
            NickName = login;
            ServerAdress = server;

            Hashtable credentials = new Hashtable();
            credentials.Add("nickname", NickName);
            credentials.Add("password", password);


            try
            {
                _connection = new ZyanConnection("tcpex://" + ServerAdress + "/Filia", _protocol, credentials, false,
                    true);
            }
            catch (SecurityException ex)
            {
                MessageBox.Show(ex.Message);
                System.Console.ReadLine();
                _isLoggedIn = false;
                return false;
            }
            catch (Exception)
            {

            }

            _connection.CallInterceptionEnabled = true;

            Filia = _connection.CreateProxy<IFilia>();
            FiliaUsers = _connection.CreateProxy<IFiliaUsers>();

            CurrentUser = FiliaUsers.GetShortUserInformation(NickName);
            _timer.Start();
            

            _isLoggedIn = true;
            return true;
        }

        public bool Logout()
        {
            if (_isLoggedIn)
            {
                _timer.Stop();
                _connection.Dispose();

                _isLoggedIn = false;
            }
            return true;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (_isLoggedIn)
            {
                FiliaUsers.IamOnline();
                CurrentUser.CopyProperties(FiliaUsers.GetShortUserInformation(NickName));
            }
        }
    }
}