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
            var dict = PlayerUtil.GetStatsDict(p);
            foreach (var kv in dict) {
                Console.WriteLine("{0}: {1}", kv.Key.ToString(), kv.Value);
            }

            Console.WriteLine("Fast stat progressions: {0}", String.Join(",", p.FastStats));
            Console.WriteLine("Slow stat progressions: {0}", String.Join(",", p.SlowStats));
            Console.WriteLine("Average stat progressions: {0}", String.Join(",", p.AverageStats));

            // double neutralStat = (double) PlayerUtil.GetStat(p.AverageStats[0], p);

            double neutralStat = 20.0;
            Console.WriteLine("Given stat: {0}", neutralStat);

            double diff = 99.0 - neutralStat;
            double levelReachMax = (diff + 1) - (double) Math.Floor(neutralStat / 5);

            double[] domain = { 1.0, levelReachMax / 4.0, levelReachMax / 2.0, (3.0*levelReachMax) / 4.0, levelReachMax };
            double[] range = { neutralStat, diff/4.0, diff/2.0, diff/1.5, 99.0 };

            Func<double, double> n = Fit.PolynomialFunc(domain, range, 2);
            //Console.WriteLine("{0} stat at levels:", p.AverageStats[0].ToString());
            Console.WriteLine("2 => {0}", n(2));
            Console.WriteLine("20 => {0}", n(20));
            Console.WriteLine("40 => {0}", n(40));
            Console.WriteLine("60 => {0}", n(65));
            Console.WriteLine("80 => {0}", n(80));
            Console.WriteLine("Hit max at {1} => {0}", n(levelReachMax), levelReachMax);

            Console.ReadKey(true);
        }
    }
}
