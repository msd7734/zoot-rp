﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using MathNet.Numerics;

using ZootRP.Core;
using ZootRP.Strings;

namespace ZootRPTesting
{
    class Program
    {
        static void PrintExpCheck(IPlayer player)
        {
            Console.WriteLine("{0}, level {1}: {2} Exp, needs {3} to next level",
                player.Identifier.CanonicalName, player.Level, player.GetLevelExp(), player.ExpToNextLevel());
        }

        static void ReportLevelUp(IPlayer sender, PlayerUpdateEventArgs e)
        {
            Console.WriteLine("{0} leveled up!", sender.Identifier.CanonicalName);
            var prevStats = PlayerUtil.GetStatValues(e.GetPlayerFromMutableState());
            var curStats = PlayerUtil.GetStatValues(sender);

            foreach (PlayerStat stat in curStats.Keys)
            {
                uint curStat = curStats[stat];
                uint prevStat = prevStats[stat];
                uint diff = curStat - prevStat;
                Console.WriteLine("{0}: {1} (+{2})", stat, curStat, diff);
            }
        }

        static void ReportReward(IPlayer sender, PlayerUpdateEventArgs e)
        {
            Console.WriteLine("{0} received:{2}{1}", sender.Identifier.CanonicalName, e.Message, Environment.NewLine);
            Console.WriteLine("{2} has: {0} Bucks and {1} level EXP", sender.Money, sender.GetLevelExp(), sender.Identifier.CanonicalName);
        }

        static void Main(string[] args)
        {
            Player p = new Player("John");
            p.LevelUpEvent += ReportLevelUp;
            p.RewardEvent += ReportReward;

            TokenDefinition[] tokenDefs = new TokenDefinition[]
            {
                new TokenDefinition(@"(?i)(health|endurance|dexterity|ingenuity|charisma|level)(?-i)","PLAYER-INT"),
                new TokenDefinition(@"(?i)(job|species|residence)(?-i)", "PLAYER-STR"),
                new TokenDefinition(@"=", "EQUALS"),
                new TokenDefinition(@"(<|>|<=|>=|=)", "COMPARATOR"),
                new TokenDefinition(@"[0-9]+", "INTEGER"),
                new TokenDefinition(@"\"".+?\""", "QUOTED-STRING"),
                new TokenDefinition(@"\s*", "SPACE"),
                new TokenDefinition(@"(&&|\|\|)", "LOGIC-BRANCH")
            };

            Lexicon nodeLexicon = new Lexicon(
                tokenDefs,
                new Dictionary<string, string>
                { 
                    { "STR-COMPARISON", "[[PLAYER-STR]][[SPACE]]=[[SPACE]][[QUOTED-STRING]]" },
                    { "INT-COMPARISON", "[[PLAYER-INT]][[SPACE]][[COMPARATOR]][[SPACE]][[INTEGER]]" },
                    { "COMPARISON", "(([[STR-COMPARISON]])|([[INT-COMPARISON]]))" },
                    { "BRANCH-EXPR", "[[COMPARISON]][[SPACE]][[LOGIC-BRANCH]][[SPACE]][[COMPARISON]]" },
                    { "MULTI-BRANCH", "[[BRANCH-EXPR]]([[SPACE]][[LOGIC-BRANCH]][[SPACE]][[COMPARISON]])+" }
                },
                false
            );

            //Console.WriteLine(nodeLexicon.MatchExpression(@"health = 12 || endurance > 10 || species = ""ape"""));
            //Console.WriteLine(nodeLexicon.MatchExpression(@"health = 12 && species = ""bunny"""));
            //Console.WriteLine(nodeLexicon.MatchExpression(@"species = ""bunny"""));

            PlayerUtil.PrintPlayerStats(p);
            Console.WriteLine("Species: {0}", p.Character.Species.Name);
            PrereqTree ptree = new PrereqTree(p, "level=1");


            // PrereqTree ptree = new PrereqTree(p, "level >= 10 && dexterity>=25 && health>=35");

            /*
            PrereqTree ptree = new PrereqTree("dexterity< 12");
            PrereqTree ptree2 = new PrereqTree("health >= 10 && charisma >= 20");
            Console.WriteLine(ptree.IsValid);
            Console.WriteLine(ptree2.IsValid);
            */

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
