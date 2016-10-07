﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics;

using ZootRP.Core;

namespace ZootRPTesting
{
    public class StatProgression : IProgression
    {
        public ProgressionRate Rate
        {
            get;
            private set;
        }

        public double Min
        {
            get;
            private set;
        }

        public double Max
        {
            get;
            private set;
        }

        public static readonly double MAXOUT_FAST = 25;
        public static readonly double MAXOUT_AVERAGE = 50;
        public static readonly double MAXOUT_SLOW = 75;

        private Func<double, double> _linearFunc;

        public StatProgression(double startStat, double maxStat, ProgressionRate rate)
        {
            Min = startStat;
            Max = maxStat;

            double targetLevel;

            switch (rate)
            {
                case ProgressionRate.Fast:
                    targetLevel = MAXOUT_FAST;
                    break;
                case ProgressionRate.Average:
                    targetLevel = MAXOUT_AVERAGE;
                    break;
                case ProgressionRate.Slow:
                    targetLevel = MAXOUT_SLOW;
                    break;
                default:
                    targetLevel = (double) Player.LEVEL_MAX;
                    break;
            }

            // shift function so that the higher starting stats reach levelMax faster
            // use equivalent ratio to (stat / MAX_LEVEL_STAT)
            // so: targetLevel * (stat / MAX_LEVEL_STAT)

            double[] x = { (double) Player.LEVEL_MIN, targetLevel };
            double[] y = { startStat, maxStat };
            this._linearFunc = Fit.LineFunc(x, y);
        }

        public double ValueAt(double level)
        {
            return this._linearFunc(level);
        }
    }
}