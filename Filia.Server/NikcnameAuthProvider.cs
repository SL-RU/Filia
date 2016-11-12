using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Zyan.Communication.Security;

namespace Filia.Server
{
    public class NicknameAuthProvider : IAuthenticationProvider
    {
        public AuthResponseMessage Authenticate(AuthRequestMessage authRequest)
        {
            if (!authRequest.Credentials.ContainsKey("nickname") || !authRequest.Credentials.ContainsKey("password"))
                return new AuthResponseMessage()
                {
                    ErrorMessage = "No valid credentials provided.",
                    Success = false
                };

            string nickname = (string)authRequest.Credentials["nickname"];
            string password = (string)authRequest.Credentials["password"];

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

            if (Program.ActiveNicknames.Contains(nickname))
                return new AuthResponseMessage()
                {
                    ErrorMessage = string.Format("Nickname '{0}' is already in use.", nickname),
                    Success = false
                };
            if (password != "kek")
                return new AuthResponseMessage()
                {
                    ErrorMessage = "WRONG PASSWORD!!!",
                    Success = false
                };

            return new AuthResponseMessage()
            {
                AuthenticatedIdentity = new GenericIdentity(nickname),
                Success = true
            };
        }
    }
}
