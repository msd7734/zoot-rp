using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics;

using ZootRP.Core;

namespace ZootRPTesting
{
    public class StatProgression : IProgression<uint>
    {
        public ProgressionType Rate
        {
            get;
            private set;
        }

        public uint Min
        {
            get;
            private set;
        }

        public uint Max
        {
            get;
            private set;
        }

        public static readonly double MAXOUT_FAST = 25;
        public static readonly double MAXOUT_AVERAGE = 50;
        public static readonly double MAXOUT_SLOW = 75;

        private Func<double, double> _linearFunc;

        public StatProgression(uint startStat, uint maxStat, ProgressionType rate)
        {
            Min = startStat;
            Max = maxStat;
            Rate = rate;

            double targetLevel;

            switch (rate)
            {
                case ProgressionType.Fast:
                    targetLevel = MAXOUT_FAST;
                    break;
                case ProgressionType.Average:
                    targetLevel = MAXOUT_AVERAGE;
                    break;
                case ProgressionType.Slow:
                    targetLevel = MAXOUT_SLOW;
                    break;
                default:
                    targetLevel = Player.LEVEL_MAX;
                    break;
            }

            // shift function so that the higher starting stats reach levelMax faster
            // use equivalent ratio to (stat / MAX_LEVEL_STAT)
            // so: targetLevel * (stat / MAX_LEVEL_STAT)

            double[] x = { (double) Player.LEVEL_MIN, targetLevel };
            double[] y = { startStat, maxStat };
            this._linearFunc = Fit.LineFunc(x, y);
        }

        public uint ValueAt(uint level)
        {
            return (uint) this._linearFunc(level);
        }
    }
}
