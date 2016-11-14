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
        string GetAllData(Action<string> answer);

        UserInformation GetUserInformation(string nickname);

        Dictionary<string, bool> GetUsersOnlineStatus();

        bool CreateNewUser(string nickname, string password, UserRole role);

    }
}
