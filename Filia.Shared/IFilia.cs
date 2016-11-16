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

    }
}
