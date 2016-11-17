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

    public class FiliaUsers : IFiliaUsers
    {
        public Server Server => Server.Instance;
        public Database Database => Server.Database;
        public FiliaAuthProvider FiliaAuth => Server.FiliaAuth;

        private ServerSession CheckUser()
        {
            var session = ServerSession.CurrentSession;
            //We though that all users here have been authenficated already.
            var v = Server.Instance.FiliaAuth.Users[session.Identity.Name];
            if (v.Role == UserRole.Blocked || v.Role == UserRole.Anonimus)
                return null;
            return session;
        }

        public void IamOnline()
        {
            var s = CheckUser();
            if (s == null)
                return;
            FiliaAuth.OnlineCallback(s.Identity.Name);
        }

        public UserInformation GetUserInformation(string nickname)
        {
            var session = CheckUser();
            if (session == null)
                return null;
            if (!Server.Instance.FiliaAuth.Users.ContainsKey(nickname)) return null;
            var u = Server.Instance.FiliaAuth.Users[nickname];
            var i = new UserInformation(u.Id, u.Nickname, u.Realname, u.About, u.Email, u.Role, u.UploadImages, u.Online);
            return i;
        }

        public bool SetUserInformation(UserInformation i)
        {
            var session = CheckUser();
            if (session == null)
                return false;
            var v = Server.Instance.FiliaAuth.Users[session.Identity.Name];
            if (v.Role != UserRole.Admin && //Admin can edit everyone
                v.Nickname != i.Nickname) //Everyone can edit yourself
                return false;
            var u = Server.Instance.FiliaAuth.Users[i.Nickname];

            if (u.Role != i.Role &&
                ((v.Role == UserRole.Admin && u.Role != UserRole.Admin && i.Role != UserRole.Admin) ||
                 (v.Role == UserRole.Moderator && i.Role > UserRole.Moderator)))
                //You can delete yourself from moderators
            {
                u.Role = i.Role;
            }

            u.Realname = i.Realname;
            u.About = i.About;
            u.Email = i.Email;
            if (v.Role == UserRole.Admin)
                u.UploadImages = i.UploadImages;

            Database.UpdateUser(u);

            return true;
        }

        public bool ChangePassword(string nickname, string password)
        {
            var session = CheckUser();
            if (session == null)
                return false;
            var v = Server.Instance.FiliaAuth.Users[session.Identity.Name];

            if (v.Role != UserRole.Admin && v.Nickname != nickname)
                return false;


            if (!Server.Instance.FiliaAuth.Users.ContainsKey(nickname))
                return false;

            var u = Server.Instance.FiliaAuth.Users[nickname];
            u.Password = FiliaAuthProvider.GetHash(password);
            Database.UpdateUser(u);

            return true;
        }

        public List<ShortUserInformation> GetShortUsersInformation()
        {
            var session = CheckUser();
            if (session == null)
                return null;

            return
                FiliaAuth.Users.Values.Select(
                    x => new ShortUserInformation(x.Id, x.Nickname, x.Realname, x.Role, x.Online)).ToList();
        }

        public ShortUserInformation GetShortUserInformation(string nickname)
        {
            var session = CheckUser();
            if (session == null)
                return null;

            if (!Server.Instance.FiliaAuth.Users.ContainsKey(nickname))
            {
                return null;
            }
            var x = Server.Instance.FiliaAuth.Users[nickname];
            return new ShortUserInformation(x.Id, x.Nickname, x.Realname, x.Role, x.Online);
        }

        public bool CreateNewUser(string nickname, string password, UserRole role)
        {
            var session = CheckUser();
            if (session == null)
                return false;
            //We though that all users here have been authenficated already.
            var v = Server.Instance.FiliaAuth.Users[session.Identity.Name];
            if (v.Role != UserRole.Admin)
                return false;
            if (role == UserRole.Admin || role == UserRole.Anonimus)
                return false;

            return FiliaAuth.CreateNewUser(nickname, password, role) != null;
        }


    }
}
