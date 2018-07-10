using System;
using System.Runtime.Serialization;

namespace FMS.Framework.Core
{
    public abstract class DateTimeBase<T> : IComparable, IFormattable, ISerializable, IComparable<T>, IEquatable<T>
        where T : DateTimeBase<T>
    {
        private DateTimeValues _dateTimeValues;

        public DateTimeBase(long ticks)
        {
            TimeConverter.ValidateTimeTicks(ticks);
            Ticks = ticks;
        }

        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);

        protected abstract long UtcTickOffset { get; }

        public long Ticks { get; }
        public int Year => DateTimeValues.Year;
        public int Month => DateTimeValues.Month;
        public int Day => DateTimeValues.Day;
        public int Hour => DateTimeValues.Hour;
        public int Minute => DateTimeValues.Minute;
        public int Second => DateTimeValues.Second;
        public int Millisecond => DateTimeValues.Millisecond;
        public int Nanosecond => DateTimeValues.Nanosecond;
        public Weekday Weekday => DateTimeValues.Weekday;

        public Date Date => new Date
        (
            DateTimeValues.Year,
            DateTimeValues.Month,
            DateTimeValues.Day,
            DateTimeValues.Weekday
        );

        public Time Time => new Time
        (
            DateTimeValues.Hour,
            DateTimeValues.Minute,
            DateTimeValues.Second,
            DateTimeValues.Millisecond,
            DateTimeValues.Nanosecond
        );

        public DateTimeValues DateTimeValues
        {
            get
            {
                lock (this)
                {
                    if (_dateTimeValues == null)
                        _dateTimeValues = TimeConverter.ToDateTimeValues(Ticks + UtcTickOffset);

                    return _dateTimeValues;
                }
            }
        }

        public int CompareTo(T other)
        {
            return other != null ? Ticks.CompareTo(other.Ticks) : -1;
        }

        public int CompareTo(object obj)
        {
            return (obj is T other) ? CompareTo(other) : -1;
        }

        public bool Equals(T other)
        {
            return other != null ? Ticks.Equals(other.Ticks) : false;
        }

        public override bool Equals(object obj)
        {
            return (obj is T other) ? Equals(other) : false;
        }

        public override int GetHashCode()
        {
            return Ticks.GetHashCode();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return DateTimeValues.ToString(format, formatProvider);
        }

        public override string ToString()
        {
            return DateTimeValues.ToString();
        }
    }
}
