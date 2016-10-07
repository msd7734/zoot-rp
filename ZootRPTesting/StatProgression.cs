using System;
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

        private static double START_LVL = 1;

        private Func<double, double> _linearFunc;

        public StatProgression(double startStat, double maxStat, ProgressionRate rate)
        {
            double targetLevel;

            switch (rate)
            {
                case ProgressionRate.Fast:
                    targetLevel = 25.0;
                    break;
                case ProgressionRate.Average:
                    targetLevel = 50.0;
                    break;
                case ProgressionRate.Slow:
                    targetLevel = 75.0;
                    break;
                default:
                    targetLevel = 100.0;
                    break;
            }

            double[] x = { START_LVL, targetLevel };
            double[] y = { startStat, maxStat };
            this._linearFunc = Fit.LineFunc(x, y);
        }

        public double ValueAt(double x)
        {
            return this._linearFunc(x);
        }
    }
}
