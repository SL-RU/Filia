using System;
using System.Collections;
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
        public FiliaAuthProvider(Server server)
        {
            Server = server;
            Users = new Dictionary<string, DbUserData>();
        }

        public Server Server { get; private set; }
        public Dictionary<string, DbUserData> Users { get; private set; }

        public void LoadUsers(List<DbUserData> users)
        {
            foreach (var user in users)
            {
                Users.Add(user.Nickname, user);
                user.Online = false;
            }
        }

        public DbUserData CreateNewUser(string nickname, string password, UserRole role)
        {
            if (Users.ContainsKey(nickname))
                return null;
            DbUserData d = new DbUserData() {Nickname = nickname, Password = GetHash(password), Role = role};
            if (!Server.Database.InsertNewUser(d))
                return null;

            Users.Add(nickname, d);
            return d;
        }

        #region auth staff

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



            if (!Users.ContainsKey(nickname))
                return new AuthResponseMessage()
                {
                    ErrorMessage = string.Format("Nickname doesn't exist."),
                    Success = false
                };
            if (StructuralComparisons.StructuralEqualityComparer.Equals(Users[nickname].Password, GetHash(password)))
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
            if (Users.ContainsKey(s.Identity.Name))
            {
                Users[s.Identity.Name].Online = true;
                Console.WriteLine("User {0} successfully logged in", s.Identity.Name);
            }
            else
            {
                Console.WriteLine("User {0} UNKNOWN LOGGED IN", s.Identity.Name);
            }
        }

        public void HostOnClientSessionTerminated(object sender, LoginEventArgs loginEventArgs)
        {
            ServerSession s = ServerSession.CurrentSession;
            if (Users.ContainsKey(s.Identity.Name))
            {
                Users[s.Identity.Name].Online = false;
                Console.WriteLine("User {0} session terminated", s.Identity.Name);
            }
            else
            {
                Console.WriteLine("User {0} UNKNOWN SESSION TERMINATED", s.Identity.Name);
            }
        }

        public void HostOnClientLogonCanceled(object sender, LoginEventArgs loginEventArgs)
        {
            ServerSession s = ServerSession.CurrentSession;
            if (Users.ContainsKey(s.Identity.Name))
            {
                Users[s.Identity.Name].Online = false;
                Console.WriteLine("User {0} logon cancelled", s.Identity.Name);
            }
            else
            {
                Console.WriteLine("User {0} UNKNOWN LOGON CANCELLED", s.Identity.Name);
            }
        }

        public void HostOnClientLoggedOff(object sender, LoginEventArgs loginEventArgs)
        {
            ServerSession s = ServerSession.CurrentSession;
            if (Users.ContainsKey(s.Identity.Name))
            {
                Users[s.Identity.Name].Online = false;
                Console.WriteLine("User {0} logged off", s.Identity.Name);
            }
            else
            {
                Console.WriteLine("User {0} UNKNOWN LOGGED OFF", s.Identity.Name);
            }
        }

        public void host_ClientHeartbeatReceived(object sender, ClientHeartbeatEventArgs e)
        {
            ServerSession s = ServerSession.CurrentSession;
            if (Users.ContainsKey(s.Identity.Name))
            {
                Users[s.Identity.Name].Online = true;
                Console.WriteLine("User {0} heartbeat", s.Identity.Name);
            }
            else
            {
                Console.WriteLine("User {0} UNKNOWN HEARTBEAT", s.Identity.Name);
            }
        }

        public static byte[] GetHash(string s)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            return sha1.ComputeHash(Encoding.UTF8.GetBytes(s));
        }

        #endregion

    }
}
