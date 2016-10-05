using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZootRP.Core;

namespace ZootRPTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PlayerStats.ExpToNextLevel(3));

            Console.ReadKey(true);
        }
    }
}
