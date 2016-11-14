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

        public UserInformation GetUserInformation(string nickname)
        {
            var session = ServerSession.CurrentSession;
            //We though that all users here have been authenficated already.
            var v = Server.Instance.FiliaAuth.Users[session.Identity.Name];
            if (v.Role == UserRole.Blocked || v.Role == UserRole.Anonimus)
                return null;
            if (!Server.Instance.FiliaAuth.Users.ContainsKey(nickname)) return null;
            var u = Server.Instance.FiliaAuth.Users[nickname];
            var i = new UserInformation(u.Id, u.Nickname, u.Realname, u.About, u.Email, u.Role, u.UploadImages, u.Online);
            return i;
        }

        public Dictionary<string, bool> GetUsersOnlineStatus()
        {
            var session = ServerSession.CurrentSession;
            //We though that all users here have been authenficated already.
            var v = Server.Instance.FiliaAuth.Users[session.Identity.Name];
            if (v.Role == UserRole.Blocked || v.Role == UserRole.Anonimus)
                return null;

            return FiliaAuth.Users.ToDictionary(u => u.Key, u => u.Value.Online);
        }

        public bool CreateNewUser(string nickname, string password, UserRole role)
        {
            var session = ServerSession.CurrentSession;
            //We though that all users here have been authenficated already.
            var v = Server.Instance.FiliaAuth.Users[session.Identity.Name];
            if (v.Role != UserRole.Admin)
                return false;
            return FiliaAuth.CreateNewUser(nickname, password, role) != null;
        }
    }
}
