using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zyan.Communication;

namespace Filia.Server
{

    public class Filia : IFilia
    {
        public event Action<string, string> MessageReceived;

        public void SendMessage(string nickname, string text)
        {
            ServerSession session = ServerSession.CurrentSession;
            Console.WriteLine(string.Format("[{0} IP={1}] {2}:{3}", DateTime.Now.ToString(), session.ClientAddress, session.Identity.Name, text));

            if (MessageReceived != null)
            {
                try
                {
                    MessageReceived(nickname, text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
