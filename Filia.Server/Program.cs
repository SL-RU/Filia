using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filia.Shared;
using Zyan.Communication;
using Zyan.Communication.Protocols.Tcp;

namespace Filia.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var auth = new FiliaAuthProvider();
            var db = new Database("db");
            db.ConnectDb();
            if (Console.ReadLine() == "n")
            {
                db.CreateDb();

                db.NewUser(new DbUserData()
                {
                    About = "admin",
                    Nickname = "admin",
                    Password = FiliaAuthProvider.GetHash("123"),
                    Role = UserRole.Admin
                });
                db.NewUser(new DbUserData()
                {
                    About = "user",
                    Nickname = "user",
                    Password = FiliaAuthProvider.GetHash("1"),
                    Role = UserRole.Translator
                });
            }


            auth.SetUsers(db.GetAllUsers());

            TcpDuplexServerProtocolSetup protocol = new TcpDuplexServerProtocolSetup(14676, auth, true);

            using (ZyanComponentHost host = new ZyanComponentHost("Filia", protocol))
            {
                host.PollingEventTracingEnabled = true;
                host.ClientHeartbeatReceived += auth.host_ClientHeartbeatReceived;

                host.RegisterComponent<IFilia, Filia>(ActivationType.Singleton);

                host.ClientLoggedOn += auth.HostOnClientLoggedOn;

                host.ClientLoggedOff += auth.HostOnClientLoggedOff;
                
                host.ClientLogonCanceled += auth.HostOnClientLogonCanceled;

                host.ClientSessionTerminated += auth.HostOnClientSessionTerminated;


                host.EnableDiscovery();
                Console.WriteLine("Chat server started. Press Enter to exit.");
                Console.ReadLine();

            }
        }
    }
}
