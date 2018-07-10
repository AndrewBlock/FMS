using System;

namespace FMS.Framework.Core
{
    public class TimeDurationValues
    {
        public TimeDurationValues(int days, int hours, int minutes, int seconds, int milliseconds)
            : this(days, hours, minutes, seconds, milliseconds, TimeConverter.NanosecondMinimum)
        {
        }

        public TimeDurationValues(int days, int hours, int minutes, int seconds, int milliseconds, int nanoseconds)
        {
            TimeConverter.ValidateDurationValues(days, hours, minutes, seconds, milliseconds, nanoseconds);

            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Milliseconds = milliseconds;
            Nanoseconds = nanoseconds;
        }

        public int Days { get; }
        public int Hours { get; }
        public int Minutes { get; }
        public int Seconds { get; }
        public int Milliseconds { get; }
        public int Nanoseconds { get; }

        private TimeSpan TimeSpan => new TimeSpan
        (
            Days,
            Hours,
            Minutes,
            Seconds,
            Milliseconds
        );

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return TimeSpan.ToString(format, formatProvider);
        }

        public override string ToString()
        {
            return TimeSpan.ToString();
        }
    }
}
