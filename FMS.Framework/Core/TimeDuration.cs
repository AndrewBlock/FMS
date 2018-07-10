using System;
using System.Runtime.Serialization;

namespace FMS.Framework.Core
{
    public class TimeDuration : IComparable, IFormattable, ISerializable, IComparable<TimeDuration>, IEquatable<TimeDuration>
    {
        public static TimeDuration Zero = new TimeDuration(0, 0, 0, 0, 0, 0);
        public static TimeDuration OneHour = new TimeDuration(0, 1, 0, 0, 0, 0);
        public static TimeDuration OneDay = new TimeDuration(1, 0, 0, 0, 0, 0);
        public static TimeDuration OneWeek = new TimeDuration(TimeConverter.DaysPerWeek, 0, 0, 0, 0, 0);

        private TimeDurationValues _timeDurationValues;

        public TimeDuration(long ticks)
        {
            TimeConverter.ValidateDurationTicks(ticks);
            Ticks = ticks;
        }

        public TimeDuration(int days, int hours, int minutes, int seconds)
            : this(days, hours, minutes, seconds, TimeConverter.MillisecondMinimum)
        {
        }

        public TimeDuration(int days, int hours, int minutes, int seconds, int milliseconds)
            : this(days, hours, minutes, seconds, milliseconds, TimeConverter.NanosecondMinimum)
        {
        }

        public TimeDuration(int days, int hours, int minutes, int seconds, int milliseconds, int nanoseconds)
        {
            Ticks = TimeConverter.ToDurationTicks(days, hours, minutes, seconds, milliseconds, nanoseconds);
        }

        public TimeDuration(SerializationInfo info, StreamingContext context)
        {
            Ticks = (long) info.GetValue(nameof(Ticks), typeof(long));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Ticks), Ticks, typeof(long));
        }

        public long Ticks { get; }
        public int Days => TimeDurationValues.Days;
        public int Hours => TimeDurationValues.Hours;
        public int Minutes => TimeDurationValues.Minutes;
        public int Seconds => TimeDurationValues.Seconds;
        public int Milliseconds => TimeDurationValues.Milliseconds;
        public int Nanoseconds => TimeDurationValues.Nanoseconds;

        protected TimeDurationValues TimeDurationValues
        {
            get
            {
                if (_timeDurationValues == null)
                {
                    _timeDurationValues = TimeConverter.ToTimeDurationValues(Ticks);
                }

                return _timeDurationValues;
            }
        }

        public static TimeDuration FromDays(int days)
        {
            return new TimeDuration(days, 0, 0, 0, 0, 0);
        }

        public static TimeDuration FromHours(int hours)
        {
            return new TimeDuration(0, hours, 0, 0, 0, 0);
        }

        public static TimeDuration FromMinutes(int minutes)
        {
            return new TimeDuration(0, 0, minutes, 0, 0, 0);
        }

        public static TimeDuration FromSeconds(int seconds)
        {
            return new TimeDuration(0, 0, 0, seconds, 0, 0);
        }

        public static TimeDuration FromMilliseconds(int milliseconds)
        {
            return new TimeDuration(0, 0, 0, 0, milliseconds, 0);
        }

        public static TimeDuration FromNanoseconds(int nanoseconds)
        {
            return new TimeDuration(0, 0, 0, 0, 0, nanoseconds);
        }

        public static TimeDuration operator +(TimeDuration duration1, TimeDuration duration2)
        {
            return new TimeDuration(duration1.Ticks + (duration2 != null ? duration2.Ticks : 0L));
        }

        public static TimeDuration operator -(TimeDuration duration1, TimeDuration duration2)
        {
            return new TimeDuration(duration1.Ticks - (duration2 != null ? duration2.Ticks : 0L));
        }

        public static TimeDuration operator *(TimeDuration duration1, int multiplyFactor)
        {
            return new TimeDuration(duration1.Ticks * multiplyFactor);
        }

        public static TimeDuration operator /(TimeDuration duration1, int divisor)
        {
            return new TimeDuration(duration1.Ticks / divisor);
        }

        public int CompareTo(TimeDuration other)
        {
            return Ticks.CompareTo(other.Ticks);
        }

        public int CompareTo(object obj)
        {
            return (obj is TimeDuration other) ? CompareTo(other) : -1;
        }

        public bool Equals(TimeDuration other)
        {
            return Ticks.Equals(other.Ticks);
        }

        public override bool Equals(object obj)
        {
            return (obj is TimeDuration other) ? Equals(other) : false;
        }

        public override int GetHashCode()
        {
            return Ticks.GetHashCode();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return TimeDurationValues.ToString(format, formatProvider);
        }

        public override string ToString()
        {
            return TimeDurationValues.ToString();
        }
    }
}
