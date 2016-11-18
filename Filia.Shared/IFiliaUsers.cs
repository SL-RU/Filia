using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filia.Shared;

namespace Filia.Server
{
    public interface IFiliaUsers
    {
        void IamOnline();

        UserInformation GetUserInformation(string nickname);

        List<ShortUserInformation> GetShortUsersInformation();
        ShortUserInformation GetShortUserInformation(string nickname);


        bool CreateNewUser(string nickname, string password, UserRole role);

        bool SetUserInformation(UserInformation information);

        bool ChangePassword(string nickname, string password);

        bool CheckNickname(string nick);
    }
}
