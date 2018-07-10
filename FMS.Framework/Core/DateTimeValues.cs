using System;

namespace FMS.Framework.Core
{
    public class DateTimeValues : IFormattable
    {
        public DateTimeValues(int year, int month, int day, int hour, int minute, int second, int millisecond)
            : this(year, month, day, hour, minute, second, millisecond, TimeConverter.NanosecondMinimum)
        {
        }

        public DateTimeValues(int year, int month, int day, int hour, int minute, int second, int millisecond, int nanosecond)
        {
            TimeConverter.ValidateDateTimeValues(year, month, day, hour, minute, second, millisecond, nanosecond);

            Year = year;
            Month = month;
            Day = day;
            Weekday = TimeConverter.ResolveWeekday(year, month, day);
            Hour = hour;
            Minute = minute;
            Second = second;
            Millisecond = millisecond;
            Nanosecond = nanosecond;
        }

        internal DateTimeValues(int year, int month, int day, Weekday weekday, int hour, int minute, int second, int millisecond, int nanosecond)
        {
            TimeConverter.ValidateDateTimeValues(year, month, day, hour, minute, second, millisecond, nanosecond);

            Year = year;
            Month = month;
            Day = day;
            Weekday = weekday;
            Hour = hour;
            Minute = minute;
            Second = second;
            Millisecond = millisecond;
            Nanosecond = nanosecond;
        }

        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public Weekday Weekday { get; }
        public int Hour { get; }
        public int Minute { get; }
        public int Second { get; }
        public int Millisecond { get; }
        public int Nanosecond { get; }

        private DateTime DateTime => new DateTime(Year, Month, Day, Hour, Minute, Second, Millisecond);

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return DateTime.ToString(format, formatProvider);
        }

        public override string ToString()
        {
            return DateTime.ToString();
        }
    }
}
