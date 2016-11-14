using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Filia.Shared;
using Zyan.Communication;
using Zyan.Communication.Security;

namespace Filia.Server
{
    public class FiliaAuthProvider : IAuthenticationProvider
    {
        public FiliaAuthProvider()
        {
            
        }

        public List<DbUserData> Users { get; private set; }

        public void SetUsers(List<DbUserData> users)
        {
            Users = users;
        }

        public AuthResponseMessage Authenticate(AuthRequestMessage authRequest)
        {
            if (!authRequest.Credentials.ContainsKey("nickname") || !authRequest.Credentials.ContainsKey("password"))
                return new AuthResponseMessage()
                {
                    ErrorMessage = "No valid credentials provided.",
                    Success = false
                };

            string nickname = (string) authRequest.Credentials["nickname"];
            string password = (string) authRequest.Credentials["password"];

            if (string.IsNullOrEmpty(nickname))
                return new AuthResponseMessage()
                {
                    ErrorMessage = "No nickname specified.",
                    Success = false
                };
            if (string.IsNullOrEmpty(password))
                return new AuthResponseMessage()
                {
                    ErrorMessage = "No password specified.",
                    Success = false
                };
            if (nickname.Length > 16 || !nickname.Any(char.IsLetterOrDigit) || password.Length > 32)
                return new AuthResponseMessage()
                {
                    ErrorMessage = "Bad nickname.",
                    Success = false
                };


            var u = Users.Find(x => x.Nickname == nickname);

            if (u == null)
                return new AuthResponseMessage()
                {
                    ErrorMessage = string.Format("Nickname doesn't exist."),
                    Success = false
                };
            if (u.Password[0] == GetHash(password)[0])
                return new AuthResponseMessage()
                {
                    AuthenticatedIdentity = new GenericIdentity(nickname),
                    Success = true
                };

            return new AuthResponseMessage()
            {
                ErrorMessage = "WRONG PASSWORD!!!",
                Success = false
            };
        }

        public void HostOnClientLoggedOn(object sender, LoginEventArgs loginEventArgs)
        {
            ServerSession s = ServerSession.CurrentSession;
            
        }

        public void HostOnClientSessionTerminated(object sender, LoginEventArgs loginEventArgs)
        {
        }

        public void HostOnClientLogonCanceled(object sender, LoginEventArgs loginEventArgs)
        {
        }

        public void HostOnClientLoggedOff(object sender, LoginEventArgs loginEventArgs)
        {
        }

        public void host_ClientHeartbeatReceived(object sender, ClientHeartbeatEventArgs e)
        {
            Console.WriteLine(string.Format("{0}: Received heartbeat from session {1}.",
                e.HeartbeatReceiveTime.ToString(), e.SessionID.ToString()));
        }

        public static byte[] GetHash(string s)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            return Encoding.UTF8.GetBytes(s);
        }
    }
}
