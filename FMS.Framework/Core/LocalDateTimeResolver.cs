using System;
using FMS.Framework.Exceptions;

namespace FMS.Framework.Core
{
    internal static class LocalDateTimeResolver
    {
        public static long ResolveUtcTicks(int year, int month, int day, int hour, int minute, int second, int millisecond, int nanosecond,
            TimeZone timeZone, TimeZoneMode? timeZoneMode)
        {
            if (timeZone == null)
                throw new ArgumentException(nameof(timeZone));

            var localTime = new UtcDateTime(year, month, day, hour, minute, second, millisecond, nanosecond);

            var standardTimeAsUtc = localTime - timeZone.GetStandardUtcOffsetForYear(year);
            var standardEffectiveTimeZonePeriod = timeZone.GetActiveTimeZonePeriod(standardTimeAsUtc);

            if (!timeZone.UsesDaylightSavingsForYear(year))
                return standardTimeAsUtc.Ticks;

            var daylightTimeAsUtc = localTime - timeZone.GetDaylightUtcOffsetForYear(year);
            var daylightEffectiveTimeZonePeriod = timeZone.GetActiveTimeZonePeriod(daylightTimeAsUtc);

            if (standardEffectiveTimeZonePeriod == daylightEffectiveTimeZonePeriod)
            {
                if (standardEffectiveTimeZonePeriod.TimeZoneMode == TimeZoneMode.StandardTime)
                    return standardTimeAsUtc.Ticks;
                else
                    return daylightTimeAsUtc.Ticks;
            }

            if (standardEffectiveTimeZonePeriod.TimeZoneMode == TimeZoneMode.StandardTime
                && daylightEffectiveTimeZonePeriod.TimeZoneMode == TimeZoneMode.DaylightTime)
            {
                if (!timeZoneMode.HasValue)
                    throw new AmbiguousDateTimeException(
                        "A valid TimeZoneMode parameter is required to clarify StandardTime or DaylightTime mode.");

                return timeZoneMode.Value == TimeZoneMode.DaylightTime
                    ? daylightTimeAsUtc.Ticks
                    : standardTimeAsUtc.Ticks;
            }

            throw new NonExistentDateTimeException("Specified local time does not exist for the given TimeZone.");
        }
    }
}
