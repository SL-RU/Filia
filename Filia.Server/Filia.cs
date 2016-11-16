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
        public Server Server => Server.Instance;
        public Database Database => Server.Database;
        public FiliaAuthProvider FiliaAuth => Server.FiliaAuth;


        public string GetAllData(Action<string> answer)
        {
            var session = ServerSession.CurrentSession;

            return session.Identity.Name;
        }
    }
}
