using System;
using System.Runtime.Serialization;

namespace FMS.Framework.Core
{
    public class LocalDateTime : DateTimeBase<LocalDateTime>
    {
        public LocalDateTime(long ticks, TimeZone timeZone)
            : base(ticks)
        {
            TimeZone = timeZone ?? throw new ArgumentException(nameof(timeZone));
            TimeZonePeriod = TimeZone.GetActiveTimeZonePeriod(UtcDateTime);
        }

        public LocalDateTime(UtcDateTime utcDateTime, TimeZone timeZone)
            : this(utcDateTime.Ticks, timeZone)
        {
        }

        public LocalDateTime(int year, int month, int day, int hour, int minute, int second,
            TimeZone timeZone, TimeZoneMode? timeZoneMode = null)
            : this(year, month, day, hour, minute, second, TimeConverter.MillisecondMinimum, timeZone, timeZoneMode)
        {
        }

        public LocalDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond,
            TimeZone timeZone, TimeZoneMode? timeZoneMode = null)
            : this(year, month, day, hour, minute, second, millisecond, TimeConverter.NanosecondMinimum, timeZone, timeZoneMode)
        {
        }

        public LocalDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int nanosecond,
            TimeZone timeZone, TimeZoneMode? timeZoneMode = null)
            : this
            (
                LocalDateTimeResolver.ResolveUtcTicks
                (
                    year,
                    month,
                    day,
                    hour,
                    minute,
                    second,
                    millisecond,
                    nanosecond,
                    timeZone,
                    timeZoneMode
                ),
                timeZone
            )
        {
        }

        public LocalDateTime(Date date, Time time, TimeZone timeZone, TimeZoneMode? timeZoneMode = null)
            : this
            (
                LocalDateTimeResolver.ResolveUtcTicks
                (
                    date.Year,
                    date.Month,
                    date.Day,
                    time.Hour,
                    time.Minute,
                    time.Second,
                    time.Millisecond,
                    time.Nanosecond,
                    timeZone,
                    timeZoneMode
                ),
                timeZone
            )
        {
        }

        protected LocalDateTime(SerializationInfo info, StreamingContext context)
            : base((long)info.GetValue(nameof(Ticks), typeof(long)))
        {
            var timeZoneIdentifier = (string) info.GetValue(nameof(TimeZone), typeof(string));

            TimeZone = TimeZone.LookupTimeZoneByIdentifier(timeZoneIdentifier);
            TimeZonePeriod = TimeZone.GetActiveTimeZonePeriod(UtcDateTime);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Ticks), Ticks, typeof(long));
            info.AddValue(nameof(TimeZone), TimeZone.Identifier, typeof(string));
        }

        protected override long UtcTickOffset => TimeZonePeriod.UtcOffset.Ticks;

        public static LocalDateTime Now => new LocalDateTime(UtcDateTime.Now, TimeZone.DefaultTimeZone);

        public TimeZone TimeZone { get; }
        public TimeZonePeriod TimeZonePeriod { get; }
        public UtcDateTime UtcDateTime => new UtcDateTime(Ticks);

        public static LocalDateTime operator +(LocalDateTime dateTime, TimeDuration duration)
        {
            return new LocalDateTime(dateTime.Ticks + (duration != null ? duration.Ticks : 0L), dateTime.TimeZone);
        }

        public static LocalDateTime operator -(LocalDateTime dateTime, TimeDuration duration)
        {
            return new LocalDateTime(dateTime.Ticks - (duration != null ? duration.Ticks : 0L), dateTime.TimeZone);
        }

        public static TimeDuration operator -(LocalDateTime dateTime1, LocalDateTime dateTime2)
        {
            if (dateTime2 == null)
                throw new ArgumentException(nameof(dateTime2));

            return new TimeDuration(dateTime1.Ticks - dateTime2.Ticks);
        }

        public override string ToString()
        {
            return $"{base.ToString()} ({TimeZonePeriod.UtcOffset})";
        }
    }
}
