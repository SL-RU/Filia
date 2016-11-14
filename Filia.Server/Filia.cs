using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Filia.Shared;
using Zyan.Communication;

namespace Filia.Server
{

    public class Filia : IFilia
    {
        public event Action<string, string> MessageReceived;

        public void SendMessage(string text)
        {
            ServerSession session = ServerSession.CurrentSession;
            Console.WriteLine(string.Format("[{0} IP={1}] {2}:{3}", DateTime.Now.ToString(), session.ClientAddress, session.Identity.Name, text));
            
            if (MessageReceived != null)
            {
                try
                {
                    MessageReceived(session.Identity.Name, text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public string GetAllData(Action<string> answer)
        {
            ServerSession session = ServerSession.CurrentSession;

            answer("LOGFFDGSDFGSDFOGSdfgkdfjgaldsjfhlajdsfhajklsfhlasdjhfladshfldshlfkadsjfasldg" +
                   "fdg" +
                   "adfga" +
                   "dfg" +
                   "adf" +
                   "gadf" +
                   "g" +
                   "adf" +
                   "g" +
                   "adf" +
                   "gadf" +
                   "g" +
                   "adf" +
                   "g" +
                   "adfg" +
                   "" +
                   "adf" +
                   "gaf" +
                   "g" +
                   "df" +
                   "ga" +
                   "dfg0dafgadfggg");
            return session.Identity.Name;
        }

        public UserInformation GetUserInformation(string nickname)
        {
            return null;
        }
    }
}
