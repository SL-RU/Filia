using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private TcpDuplexServerProtocolSetup Protocol;
        private ZyanComponentHost Host;
        public static Server Instance { get; private set; }

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

            Protocol = new TcpDuplexServerProtocolSetup(14676, FiliaAuth, true);

            Host = new ZyanComponentHost("Filia", Protocol);
            Host.PollingEventTracingEnabled = true;
            Host.ClientHeartbeatReceived += FiliaAuth.host_ClientHeartbeatReceived;

            Host.RegisterComponent<IFilia, Filia>(ActivationType.Singleton);

            Host.ClientLoggedOn += FiliaAuth.HostOnClientLoggedOn;

            Host.ClientLoggedOff += FiliaAuth.HostOnClientLoggedOff;

            Host.ClientLogonCanceled += FiliaAuth.HostOnClientLogonCanceled;

            Host.ClientSessionTerminated += FiliaAuth.HostOnClientSessionTerminated;


            Host.EnableDiscovery();

            Console.WriteLine("Chat server started.");

        }

        public void Stop()
        {
            Host.DisableDiscovery();
            Database.CloseDb();
            Host.Dispose();
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
