using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filia.Shared;
using Zyan.Communication;
using Zyan.Communication.Protocols.Tcp;

namespace Filia.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server s = new Server("db");
            if (Console.ReadLine() == "n")
            {
                if (File.Exists("db"))
                {
                    File.Delete("db");
                }
            }

            s.Start();
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
            s.Stop();
        }
    }
}
