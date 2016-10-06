using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics;

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

            Console.WriteLine("Fast stat progressions: {0}", String.Join(",", p.FastStats));
            Console.WriteLine("Slow stat progressions: {0}", String.Join(",", p.SlowStats));
            Console.WriteLine("Average stat progressions: {0}", String.Join(",", p.AverageStats));

            uint neutralStat = PlayerStats.GetStat(p.AverageStats[0], p);

            double[] domain = { 1.0, 25.5, 50, 75.5, 99.0 };
            double diff = 99.0 - (double)neutralStat;
            double[] range = { (double)neutralStat, diff/4.0, diff/2.0, diff/1.5, 99.0 };
            double[] poly = Fit.Polynomial(domain, range, 2);

            Func<double, double> n = Fit.PolynomialFunc(domain, range, 2);
            Console.WriteLine("{0} stat at levels:", p.AverageStats[0].ToString());
            Console.WriteLine("1 => {0}", n(1));
            Console.WriteLine("25 => {0}", n(25));
            Console.WriteLine("50 => {0}", n(50));
            Console.WriteLine("75 => {0}", n(99));


            Console.WriteLine("Progression function: {0}", String.Join(" + ", poly));

            Console.ReadKey(true);
        }
    }
}
