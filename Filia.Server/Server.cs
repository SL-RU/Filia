using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading;
using Filia.Shared;
using Zyan.Communication;
using Zyan.Communication.Protocols.Tcp;

namespace Filia.Server
{
    public class Server
    {
        public Server(string path)
        {
            FiliaAuth = new FiliaAuthProvider(this);
            Database = new Database("db");
            Instance = this;
            
               
        }

        public FiliaAuthProvider FiliaAuth { get; private set; }
        public Database Database { get; private set; }
        public static Server Instance { get; private set; }
        public Filia Filia { get; private set; }

        private TcpDuplexServerProtocolSetup _protocol;
        private ZyanComponentHost _host;

        public void Start()
        {
            if (File.Exists(Database.DbPath))
            {
                Database.ConnectDb();
                Console.WriteLine("Database found. Connected.");
            }
            else
            {
                Console.WriteLine("Database not found.");
                FirstStart();
            }

            FiliaAuth.LoadUsers(Database.GetAllUsers());

            _protocol = new TcpDuplexServerProtocolSetup(14676, FiliaAuth, true);

            _host = new ZyanComponentHost("Filia", _protocol);
            _host.PollingEventTracingEnabled = true;
            _host.ClientHeartbeatReceived += FiliaAuth.host_ClientHeartbeatReceived;

            _host.RegisterComponent<IFilia, Filia>(ActivationType.Singleton);
            _host.RegisterComponent<IFiliaUsers, FiliaUsers>(ActivationType.Singleton);

            _host.ClientLoggedOn += FiliaAuth.HostOnClientLoggedOn;

            _host.ClientLoggedOff += FiliaAuth.HostOnClientLoggedOff;

            _host.ClientLogonCanceled += FiliaAuth.HostOnClientLogonCanceled;

            _host.ClientSessionTerminated += FiliaAuth.HostOnClientSessionTerminated;

            _host.EnableDiscovery();


            Console.WriteLine("Chat server started.");

        }

        public void Stop()
        {
            _host.DisableDiscovery();
            Database.CloseDb();
            _host.Dispose();

        }

        private void FirstStart()
        {
            Console.WriteLine("Creating.");

            Database.ConnectDb();
            Database.CreateDb();

            string pswd = "1";//Guid.NewGuid().ToString().Substring(0, 5);
            Database.InsertNewUser(new DbUserData()
            {
                About = "admin",
                Nickname = "admin",
                Password = FiliaAuthProvider.GetHash(pswd),
                Role = UserRole.Admin
            });
            Console.WriteLine("User admin created. Password {0}", pswd);
        }
    }
}
