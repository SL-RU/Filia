using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filia.Shared;

namespace Filia.Server
{
    public interface IFilia
    {
        event Action<string, string> MessageReceived;

        void SendMessage(string text);

        string GetAllData(Action<string> answer);

        UserInformation GetUserInformation(string nickname);
    }
}
