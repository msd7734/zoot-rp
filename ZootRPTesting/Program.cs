﻿using System;
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
        static void PrintExpCheck(IPlayer player)
        {
            Console.WriteLine("{0}, level {1}: {2} Exp, needs {3} to next level",
                player.Identifier.CanonicalName, player.Level, player.GetLevelExp(), player.ExpToNextLevel());
        }

        static void ReportLevelUp(IPlayer sender, EventArgs e)
        {
            Console.WriteLine("{0} leveled up!", sender.Identifier.CanonicalName);
        }

        static void Main(string[] args)
        {
            Player p = new Player("John");

            PlayerUtil.PrintPlayerStats(p, true);
            PrintExpCheck(p);

            Console.WriteLine("Player health: {0}", p.GetHealth());

            p.LevelUpEvent += ReportLevelUp;



            p.AwardLevelExp(300);
            PlayerUtil.PrintPlayerStats(p, false);
            PrintExpCheck(p);

            /*
            Console.WriteLine("Fast stat progressions: {0}", String.Join(",", p.FastStats));
            Console.WriteLine("Slow stat progressions: {0}", String.Join(",", p.SlowStats));
            Console.WriteLine("Average stat progressions: {0}", String.Join(",", p.AverageStats));

            // double neutralStat = (double) PlayerUtil.GetStat(p.AverageStats[0], p);

            double neutralStat = 15.0;
            Console.WriteLine("Given stat: {0}", neutralStat);
            */
            

            // double diff = MAX_STAT - neutralStat;
            // double levelReachMax = (diff + 1) - (double) Math.Floor(neutralStat / 5);

            // fast, reaches 50 naturally by 25
            // avg, reaches 50 naturally by 50
            // slow, reaches 50 naturally by 75

            /*
            double MAX_LEVEL = 100.0;
            double MAX_STAT = 50.0;

            double targetLvl = 25;

            double neutralStat = 15.0;
            Console.WriteLine("Given stat: {0}", neutralStat);

            // double[] domain = { 1.0, targetLvl / 4, targetLvl / 2, (3 * targetLvl) / 4, targetLvl };
            // double[] range = { neutralStat, MAX_STAT/4, MAX_STAT / 2, (3*MAX_STAT)/4, MAX_STAT };

            double[] domain = { 1.0, targetLvl };
            double[] range = { neutralStat, MAX_STAT};

            Func<double, double> n = Fit.LineFunc(domain, range);
            //Console.WriteLine("{0} stat at levels:", p.AverageStats[0].ToString());
            Console.WriteLine("1 => {0}", n(1));
            Console.WriteLine("5 => {0}", n(5));
            Console.WriteLine("10 => {0}", n(10));
            Console.WriteLine("15 => {0}", n(15));
            Console.WriteLine("20 => {0}", n(20));
            Console.WriteLine("25 => {0}", n(25));
            Console.WriteLine("{1} => {0}", n(targetLvl), targetLvl);
            */
            Console.ReadKey(true);
        }
    }
}
