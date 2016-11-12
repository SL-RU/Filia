using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zyan.Communication;
using Zyan.Communication.Protocols.Tcp;

namespace Filia.Server
{
    class Program
    {
        public static List<string> ActiveNicknames { get; set; }

        static void Main(string[] args)
        {
            ActiveNicknames = new List<string>();

            TcpDuplexServerProtocolSetup protocol = new TcpDuplexServerProtocolSetup(14676, new NicknameAuthProvider(),
                true);

            using (ZyanComponentHost host = new ZyanComponentHost("Filia", protocol))
            {
                host.PollingEventTracingEnabled = true;
                host.ClientHeartbeatReceived += new EventHandler<ClientHeartbeatEventArgs>(host_ClientHeartbeatReceived);

                host.RegisterComponent<IFilia, Filia>(ActivationType.Singleton);

                host.ClientLoggedOn += new EventHandler<LoginEventArgs>((sender, e) =>
                {
                    Console.WriteLine("{0}: User '{1}' with IP {2} logged on.", e.Timestamp.ToString(),
                        e.Identity.Name, e.ClientAddress);
                    ActiveNicknames.Add(e.Identity.Name);
                });

                host.ClientLoggedOff += new EventHandler<LoginEventArgs>((sender, e) =>
                {
                    Console.WriteLine(string.Format("{0}: User '{1}' with IP {2} logged off.", e.Timestamp.ToString(),
                        e.Identity.Name, e.ClientAddress));
                    ActiveNicknames.Remove(e.Identity.Name);
                });

                host.ClientSessionTerminated += new EventHandler<LoginEventArgs>((sender, e) =>
                {
                    Console.WriteLine(string.Format("{0}: User '{1}' with IP {2} was kicked due to inactivity.",
                        e.Timestamp.ToString(), e.Identity.Name, e.ClientAddress));
                    ActiveNicknames.Remove(e.Identity.Name);
                });


                host.EnableDiscovery();
                Console.WriteLine("Chat server started. Press Enter to exit.");
                Console.ReadLine();

            }
        }

        static void host_ClientHeartbeatReceived(object sender, ClientHeartbeatEventArgs e)

        {
            Console.WriteLine(string.Format("{0}: Received heartbeat from session {1}.",
                e.HeartbeatReceiveTime.ToString(), e.SessionID.ToString()));
        }
    }
}
