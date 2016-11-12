using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filia.Server
{
    public interface IFilia
    {
        event Action<string, string> MessageReceived;

        void SendMessage(string nickname, string text);
    }
}
