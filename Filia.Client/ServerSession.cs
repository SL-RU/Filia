using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Filia.Server;
using Zyan.Communication;
using Zyan.Communication.Protocols.Tcp;

namespace Filia.Client
{
    public class ServerSession
    {
        public ServerSession()
        {
            _protocol = new TcpDuplexClientProtocolSetup(true);
        }

        private ZyanConnection _connection = null;
        private TcpDuplexClientProtocolSetup _protocol;
        private IFilia _filiaProxy;
        private string _nickName = "",
            _serverAdress = "";

        private bool _isLoggedIn;

        public bool IsLoggedIn => _isLoggedIn && _connection != null && _connection.IsSessionValid;
        public string ServerAdress => _serverAdress;
        public string NickName => _nickName;
        public IFilia FiliaProxy => _filiaProxy;

        public bool Login(string login, string server, string password)
        {
            _nickName = login;
            _serverAdress = server;

            Hashtable credentials = new Hashtable();
            credentials.Add("nickname", _nickName);
            credentials.Add("password", password);


            try
            {
                _connection = new ZyanConnection("tcpex://" + _serverAdress + "/Filia", _protocol, credentials, false, true);
            }
            catch (SecurityException ex)
            {
                MessageBox.Show(ex.Message);
                System.Console.ReadLine();
                _isLoggedIn = false;
                return false;
            }

            _connection.CallInterceptionEnabled = true;

            _filiaProxy = _connection.CreateProxy<IFilia>();
            //_filiaProxy.MessageReceived += new Action<string, string>(FiliaRecieved);


            _isLoggedIn = true;
            return true;
        }

        public bool Logout()
        {
            //_filiaProxy.MessageReceived -= new Action<string, string>(FiliaRecieved);
            _connection.Dispose();

            return true;
        }
    }
}
