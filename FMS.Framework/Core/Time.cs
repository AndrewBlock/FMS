using System;
using System.Runtime.Serialization;

namespace FMS.Framework.Core
{
    public class Time : IComparable, IFormattable, ISerializable, IComparable<Time>, IEquatable<Time>
    {
        public Time(int hour, int minute, int second)
            : this(hour, minute, second, TimeConverter.MillisecondMinimum)
        {
        }

        public Time(int hour, int minute, int second, int millisecond)
            : this(hour, minute, second, millisecond, TimeConverter.NanosecondMinimum)
        {
        }

        public Time(int hour, int minute, int second, int millisecond, int nanosecond)
        {
            TimeConverter.ValidateTimeValues(hour, minute, second, millisecond, nanosecond);

            Hour = hour;
            Minute = minute;
            Second = second;
            Millisecond = millisecond;
            Nanosecond = nanosecond;
        }

        protected Time(SerializationInfo info, StreamingContext context)
        {
            Hour = (int) info.GetValue(nameof(Hour), typeof(int));
            Minute = (int) info.GetValue(nameof(Minute), typeof(int));
            Second = (int) info.GetValue(nameof(Second), typeof(int));
            Millisecond = (int) info.GetValue(nameof(Millisecond), typeof(int));
            Nanosecond = (int) info.GetValue(nameof(Nanosecond), typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Hour), Hour, typeof(int));
            info.AddValue(nameof(Minute), Minute, typeof(int));
            info.AddValue(nameof(Second), Second, typeof(int));
            info.AddValue(nameof(Millisecond), Millisecond, typeof(int));
            info.AddValue(nameof(Nanosecond), Nanosecond, typeof(int));
        }

        public int Hour { get; }
        public int Minute { get; }
        public int Second { get; }
        public int Millisecond { get; }
        public int Nanosecond { get; }

        public static bool operator <(Time time1, Time time2) => time1.CompareTo(time2) == -1;
        public static bool operator <=(Time time1, Time time2) => time1.CompareTo(time2) != 1;
        public static bool operator >(Time time1, Time time2) => time1.CompareTo(time2) == 1;
        public static bool operator >=(Time time1, Time time2) => time1.CompareTo(time2) != -1;

        public int CompareTo(object obj) => CompareTo(obj as Time);

        public int CompareTo(Time other)
        {
            if (other == null)
                return -1;

            if (Hour > other.Hour)
                return 1;
            if (Hour < other.Hour)
                return -1;
            if (Minute > other.Minute)
                return 1;
            if (Minute < other.Minute)
                return -1;
            if (Second > other.Second)
                return 1;
            if (Second < other.Second)
                return -1;
            if (Millisecond > other.Millisecond)
                return 1;
            if (Millisecond < other.Millisecond)
                return -1;
            if (Nanosecond > other.Nanosecond)
                return 1;
            if (Nanosecond < other.Nanosecond)
                return -1;

            return 0;
        }

        public bool Equals(Time other) => CompareTo(other) == 0;
        public override bool Equals(object obj) => Equals(obj as Time);
        public override int GetHashCode() => Hour ^ Minute ^ Second ^ Millisecond;

        private DateTime DateTime => new DateTime
        (
            TimeConverter.YearMinimum,
            TimeConverter.MonthMinimum,
            TimeConverter.DayMinimum,
            Hour,
            Minute,
            Second,
            Millisecond
        );

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return DateTime.ToString(format, formatProvider);
        }

        public override string ToString()
        {
            return DateTime.ToString("T");
        }
    }
}
