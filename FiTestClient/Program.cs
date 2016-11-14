using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Filia.Server;
using Zyan.Communication;
using Zyan.Communication.Protocols.Tcp;

namespace FiTestClient
{
    class Program
    {
        private static string _nickName = string.Empty,
            server = "", password = "";

        static void Main(string[] args)
        {
            System.Console.Write("Nickname: ");
            _nickName = System.Console.ReadLine();
            System.Console.Write("Password: ");
            password = System.Console.ReadLine();
            Console.Write("\nServer: ");
            server = Console.ReadLine();
            if (server == "")
                server = "localhost";
            System.Console.WriteLine("-----------------------------------------------");

            Hashtable credentials = new Hashtable();
            credentials.Add("nickname", _nickName);
            credentials.Add("password", password);

            ZyanConnection connection = null;
            TcpDuplexClientProtocolSetup protocol = new TcpDuplexClientProtocolSetup(true);

            try
            {
                connection = new ZyanConnection("tcpex://" + server + ":14676/Filia", protocol, credentials, false, true);
            }
            catch (SecurityException ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Console.ReadLine();
                return;
            }

            connection.CallInterceptors.For<IFilia>().Add(
                (IFilia chat, string nickname, string message) => chat.SendMessage(message),
                (data, nickname, message) =>
                {
                    if (message.Contains("fuck") || message.Contains("sex"))
                    {
                        System.Console.WriteLine("TEXT CONTAINS FORBIDDEN WORDS!");
                        data.Intercepted = true;
                    }
                });

            connection.CallInterceptionEnabled = true;

            IFilia chatProxy = connection.CreateProxy<IFilia>();
            chatProxy.MessageReceived += new Action<string, string>(chatProxy_MessageReceived);

            string text = string.Empty;

            while (text.ToLower() != "quit")
            {
                text = System.Console.ReadLine();
                chatProxy.SendMessage(text);
            }

            chatProxy.MessageReceived -= new Action<string, string>(chatProxy_MessageReceived);
            connection.Dispose();
        }

        private static void chatProxy_MessageReceived(string arg1, string arg2)
        {
            if (arg1 != _nickName)
                System.Console.WriteLine(string.Format("{0}: {1}", arg1, arg2));
        }
    }
}
