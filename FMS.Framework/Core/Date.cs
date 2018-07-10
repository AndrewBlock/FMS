using System;
using System.Runtime.Serialization;

namespace FMS.Framework.Core
{
    public class Date : IComparable, IFormattable, ISerializable, IComparable<Date>, IEquatable<Date>
    {
        public Date(int year, int month, int day)
        {
            TimeConverter.ValidateDate(year, month, day);

            Year = year;
            Month = month;
            Day = day;
            Weekday = TimeConverter.ResolveWeekday(Year, Month, Day);
        }

        internal Date(int year, int month, int day, Weekday weekday)
        {
            TimeConverter.ValidateDate(year, month, day);

            Year = year;
            Month = month;
            Day = day;
            Weekday = weekday;
        }

        protected Date(SerializationInfo info, StreamingContext context)
        {
            Year = (int) info.GetValue(nameof(Year), typeof(int));
            Month = (int) info.GetValue(nameof(Month), typeof(int));
            Day = (int) info.GetValue(nameof(Day), typeof(int));
            Weekday = TimeConverter.ResolveWeekday(Year, Month, Day);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Year), Year, typeof(int));
            info.AddValue(nameof(Month), Month, typeof(int));
            info.AddValue(nameof(Day), Day, typeof(int));
        }

        public int Year { get; }
        public int Month { get; }
        public int Day { get; }
        public Weekday Weekday { get; }

        public Date NextDay => TimeConverter.IncrementDays(this, 1);
        public Date PreviousDay => TimeConverter.IncrementDays(this, -1);
        public Date AddDays(int dayCount) => TimeConverter.IncrementDays(this, dayCount);
        public Date SubractDays(int dayCount) => TimeConverter.IncrementDays(this, -dayCount);

        public static bool operator <(Date date1, Date date2) => date1.CompareTo(date2) == -1;
        public static bool operator <=(Date date1, Date date2) => date1.CompareTo(date2) != 1;
        public static bool operator >(Date date1, Date date2) => date1.CompareTo(date2) == 1;
        public static bool operator >=(Date date1, Date date2) => date1.CompareTo(date2) != -1;

        public int CompareTo(object obj) => CompareTo(obj as Date);

        public int CompareTo(Date other)
        {
            if (other == null)
                return -1;

            if (Year > other.Year)
                return 1;
            if (Year < other.Year)
                return -1;
            if (Month > other.Month)
                return 1;
            if (Month < other.Month)
                return -1;
            if (Day > other.Day)
                return 1;
            if (Day < other.Day)
                return -1;
            return 0;
        }

        public bool Equals(Date other) => CompareTo(other) == 0;
        public override bool Equals(object obj) => Equals(obj as Date);
        public override int GetHashCode() => Year ^ Month ^ Day;

        private DateTime DateTime => new DateTime
        (
            Year,
            Month,
            Day,
            TimeConverter.HourMinimum,
            TimeConverter.MinuteMinimum,
            TimeConverter.SecondMinimum,
            TimeConverter.MillisecondMinimum
        );

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return DateTime.ToString(format, formatProvider);
        }

        public override string ToString()
        {
            return DateTime.ToString("d");
        }
    }
}
