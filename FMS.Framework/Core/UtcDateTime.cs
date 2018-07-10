using System;
using System.Runtime.Serialization;

namespace FMS.Framework.Core
{
    public class UtcDateTime : DateTimeBase<UtcDateTime>
    {
        public static UtcDateTime MinValue = new UtcDateTime(TimeConverter.TimeTicksMinimum);
        public static UtcDateTime MaxValue = new UtcDateTime(TimeConverter.TimeTicksMaximum);

        public UtcDateTime(long ticks)
            : base(ticks)
        {
        }

        public UtcDateTime(int year, int month, int day, int hour, int minute, int second)
            : this(year, month, day, hour, minute, second, TimeConverter.MillisecondMinimum)
        {
        }

        public UtcDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
            : this(year, month, day, hour, minute, second, millisecond, TimeConverter.NanosecondMinimum)
        {
        }

        public UtcDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int nanosecond)
            : this
            (
                TimeConverter.ToTimeTicks
                (
                    year,
                    month,
                    day,
                    hour,
                    minute,
                    second,
                    millisecond,
                    nanosecond
                 )
             )
        {
        }

        public UtcDateTime(Date date, Time time)
            : this
            (
                TimeConverter.ToTimeTicks
                (
                    date.Year,
                    date.Month,
                    date.Day,
                    time.Hour,
                    time.Minute,
                    time.Second,
                    time.Millisecond,
                    time.Nanosecond
                )
            )
        {
        }

        protected UtcDateTime(SerializationInfo info, StreamingContext context)
            : base((long) info.GetValue(nameof(Ticks), typeof(long)))
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Ticks), Ticks, typeof(long));
        }

        protected override long UtcTickOffset => 0L;

        public static UtcDateTime Now
        {
            get
            {
                Interop.WinApi.GetSystemTime(out Interop.SystemTime systemTime);

                return new UtcDateTime
                (
                    systemTime.Year,
                    systemTime.Month,
                    systemTime.Day,
                    systemTime.Hour,
                    systemTime.Minute,
                    systemTime.Second,
                    systemTime.Milliseconds
                );
            }
        }

        public static UtcDateTime operator +(UtcDateTime dateTime, TimeDuration duration)
        {
            return new UtcDateTime(dateTime.Ticks + (duration != null ? duration.Ticks : 0L));
        }

        public static UtcDateTime operator -(UtcDateTime dateTime, TimeDuration duration)
        {
            return new UtcDateTime(dateTime.Ticks - (duration != null ? duration.Ticks : 0L));
        }

        public static TimeDuration operator -(UtcDateTime dateTime1, UtcDateTime dateTime2)
        {
            if (dateTime2 == null)
                throw new ArgumentException(nameof(dateTime2));

            return new TimeDuration(dateTime1.Ticks - dateTime2.Ticks);
        }

        public override int GetHashCode()
        {
            return Ticks.GetHashCode();
        }
    }
}
