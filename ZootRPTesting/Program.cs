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
            Player p = new Player("John");
            var dict = PlayerStats.GetStatsDict(p);
            foreach (var kv in dict) {
                Console.WriteLine("{0}: {1}", kv.Key.ToString(), kv.Value);
            }
            Console.ReadKey(true);
        }
    }
}
