using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NCrontab;

namespace ZootRP.Core.Prereq
{
    /// <summary>
    /// Determines what time period in the cron string is relevant to determining whether the time prerequisite is met.
    /// </summary>
    public enum TimeFrequency
    {
        Forever,
        PerHour,
        PerDay,
        PerDayOfWeek,
        PerWeek,
        PerMonth,
        PerYear
    }

    public class TimePrerequisite : IPrerequisite
    {

        private CrontabSchedule _schedule;
        private TimeFrequency _frequency;
        private int _timesAllowed;

        public string Value
        {
            get;
            private set;
        }

        /// <summary>
        /// Define a time, some day(s), that must be current to fulfill the prerequesite.
        /// </summary>
        /// <param name="cron">A cron string that defines the prerequesite interval.</param>
        public TimePrerequisite(string cron)
        {
            Value = cron;
            this._schedule = CrontabSchedule.Parse(cron);
            this._frequency = TimeFrequency.Forever;
            this._timesAllowed = -1;
        }

        /// <summary>
        /// Define a time, some day(s), that must be current to fulfill the prerequesite.
        /// </summary>
        /// <param name="cron">A cron string that defines the prerequesite interval.</param>
        /// <param name="frequencyAllowed">While the cron string represents the current time, limit the number of
        /// times the prerequisite is met per <c>frequencyAllowed</c>.
        /// </param>
        public TimePrerequisite(string cron, TimeFrequency frequencyAllowed)
        {
            Value = cron;
            this._schedule = CrontabSchedule.Parse(cron);
            this._frequency = frequencyAllowed;
            this._timesAllowed = -1;
        }

        /// <summary>
        /// Define a time, some day(s), that must be current to fulfill the prerequesite.
        /// </summary>
        /// <param name="cron">A cron string that defines the prerequesite interval.</param>
        /// <param name="frequencyAllowed">While the cron string represents the current time, limit the number of
        /// times the prerequisite is met per <c>frequencyAllowed</c>.
        /// </param>
        /// <param name="timesAllowed">The numer of times the prerequssite can be fulfilled per <c>frequencyAllowed</c>.
        /// A value less than 1 indicates no limit.
        /// </param>
        public TimePrerequisite(string cron, TimeFrequency frequencyAllowed, int timesAllowed)
        {
            Value = cron;
            this._schedule = CrontabSchedule.Parse(cron);
            this._frequency = frequencyAllowed;
            this._timesAllowed = (timesAllowed > 0) ? timesAllowed : -1;
        }

        public bool PlayerMeets(IPlayer player)
        {
            // Here's how this is broken down:
            //  - The cronstring determines which times are valid
            //  - Frequency determines which time divisions during those times are to be compared with today
            //  - The timesAllowed determines how many times in that time division the player can do the activity
            //  - NOTE: I'm still not convinced timesAllowed should be a part of this, maybe Activity implementations should handle that? Or something else?
            //          Maybe wrap this in another object!

            // Example: cron defines the 5th of each month
            // Frequency = PerHour
            // timesAllowed = 1
            // This means that, on the 5th of each month, the task can be done 1 time per hour.

            var occurence = this._schedule.GetNextOccurrence(DateTime.Today);
            var today = DateTime.Today;

            // do this to determine what granularity to compare
            // TODO: also need to check against the times allowed
            switch (this._frequency)
            {
                case TimeFrequency.PerHour:
                    return (occurence.Hour == today.Hour);
                case TimeFrequency.PerDay:
                    return (occurence.Day == today.Day);
                case TimeFrequency.PerDayOfWeek:
                    return (occurence.DayOfWeek == today.DayOfWeek);
                case TimeFrequency.PerMonth:
                    return (occurence.Month == today.Month);
                case TimeFrequency.PerYear:
                    return (occurence.Year == today.Year);
                case TimeFrequency.Forever:
                default:
                    return true;
            }
        }
    }
}
