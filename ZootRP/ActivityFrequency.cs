using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZootRP.Core
{
    public enum Frequency
    {
        Unlimited,
        Daily,
        Weekly,
        Monthly,
        WeeklyRandom,
        MonthlyRandom,
        Date
    }

    public class ActivityFrequency
    {
        public Frequency Freq
        {
            get;
            private set;
        }

        public DateTime When
        {
            get;
            private set;
        }

        private ActivityFrequency(Frequency freq)
        {
            Freq = freq;
            When = new DateTime();
        }

        private ActivityFrequency(Frequency freq, DateTime time)
        {
            Freq = freq;
            When = time;
        }

        public static ActivityFrequency Unlimited()
        {
            return new ActivityFrequency(Frequency.Unlimited);
        }

        public static ActivityFrequency Daily()
        {
            return new ActivityFrequency(Frequency.Daily);
        }

        public static ActivityFrequency Weekly(DayOfWeek day)
        {
            DateTime now = DateTime.Now;
            DateTime d;
            if (now.DayOfWeek != day)
            {
                int daysUntilNext = ((int)day - (int)now.DayOfWeek + 7) % 7;
                d = now.AddDays(daysUntilNext);
            }
            else
                d = now;

            return new ActivityFrequency(Frequency.Weekly, d);
        }

        public static ActivityFrequency Monthly(int day)
        {
            DateTime now = DateTime.Now;
            DateTime d;
            if (now.Day != day)
            {
                int daysUntilNext;
                if (now.Day > day)
                {
                    // the next day of the activity is next month
                    daysUntilNext = 0;
                }
                else
                {
                    // the next day of the activity is later this month
                    daysUntilNext = day - now.Day;
                }
            }
            
            return new ActivityFrequency(Frequency.Monthly);
        }

        public static ActivityFrequency Date(DateTime date)
        {
            return new ActivityFrequency(Frequency.Date, date);
        }

        public static ActivityFrequency WeeklyRandom()
        {
            return new ActivityFrequency(Frequency.WeeklyRandom);
        }

        public static ActivityFrequency MonthlyRandom()
        {
            return new ActivityFrequency(Frequency.MonthlyRandom);
        }

        public bool IsAvailableNow()
        {
            DateTime now = DateTime.Now;

            switch (Freq)
            {
                case Frequency.Unlimited:
                case Frequency.Daily:
                case Frequency.WeeklyRandom:
                case Frequency.MonthlyRandom:
                    return true;
                case Frequency.Weekly:
                case Frequency.Monthly:
                case Frequency.Date:
                default:
                    return false;
            }
        }
    }
}
