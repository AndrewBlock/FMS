using System.Collections.Generic;
using FMS.Framework.Interop;

namespace FMS.Framework.Core
{
    public class TimeZonePeriod
    {
        private TimeZonePeriod
        (
            int year,
            DateTimeValues localTransitionTime,
            TimeDuration transitionOffset,
            UtcDateTime utcPeriodStart,
            TimeDuration utcOffset,
            TimeZoneMode timeZoneMode
        )
        {
            Year = year;
            LocalTransitionTime = localTransitionTime;
            TransitionOffset = transitionOffset;
            UtcPeriodStart = utcPeriodStart;
            UtcOffset = utcOffset;
            TimeZoneMode = timeZoneMode;
        }

        public int Year { get; }
        public DateTimeValues LocalTransitionTime { get; }
        public TimeDuration TransitionOffset { get; }
        public UtcDateTime UtcPeriodStart { get; }
        public TimeDuration UtcOffset { get; }
        public TimeZoneMode TimeZoneMode { get; }

        internal static TimeZonePeriod GetForcedStandardTimeZonePeriod(UtcDateTime dateTime, Tzi defaultDstRule)
        {
            var utcMinValue = UtcDateTime.MinValue;

            return new TimeZonePeriod
            (
                dateTime.Year,
                dateTime.DateTimeValues,
                TimeDuration.Zero,
                dateTime,
                TimeDuration.FromMinutes(-(defaultDstRule.Bias + defaultDstRule.StandardBias)),
                TimeZoneMode.StandardTime
            );
        }

        internal static IEnumerable<TimeZonePeriod> GetTimeZonePeriodsForYear(int year, Tzi dstRule, Tzi prevYearDstRule)
        {
            if (dstRule.StandardDate.Month == 0 && dstRule.DaylightDate.Month == 0)
            {
                return new List<TimeZonePeriod>();
            }

            if (dstRule.DaylightDate.Month == 0)
            {
                return new[]
                {
                    GenerateTimeZonePeriod
                    (
                        year,
                        dstRule.StandardDate,
                        TziUtils.GetStandardTimeUtcOffset(dstRule),
                        TziUtils.GetYearFinalUtcOffset(prevYearDstRule),
                        TimeZoneMode.StandardTime
                    )
                };
            }

            if (dstRule.StandardDate.Month == 0)
            {
                return new[]
                {
                    GenerateTimeZonePeriod
                    (
                        year,
                        dstRule.DaylightDate,
                        TziUtils.GetDaylightTimeUtcOffset(dstRule),
                        TziUtils.GetYearFinalUtcOffset(prevYearDstRule),
                        TimeZoneMode.DaylightTime
                    )
                };
            }

            if (dstRule.DaylightDate.Month < dstRule.StandardDate.Month)
            {
                return new []
                {
                    GenerateTimeZonePeriod
                    (
                        year,
                        dstRule.DaylightDate,
                        TziUtils.GetDaylightTimeUtcOffset(dstRule),
                        TziUtils.GetYearFinalUtcOffset(prevYearDstRule),
                        TimeZoneMode.DaylightTime
                    ),
                    GenerateTimeZonePeriod
                    (
                        year,
                        dstRule.StandardDate,
                        TziUtils.GetStandardTimeUtcOffset(dstRule),
                        TziUtils.GetDaylightTimeUtcOffset(dstRule),
                        TimeZoneMode.StandardTime
                    )
                };
            }
            else
            {
                return new []
                {
                    GenerateTimeZonePeriod
                    (
                        year,
                        dstRule.StandardDate,
                        TziUtils.GetStandardTimeUtcOffset(dstRule),
                        TziUtils.GetYearFinalUtcOffset(prevYearDstRule),
                        TimeZoneMode.StandardTime
                    ),
                    GenerateTimeZonePeriod
                    (
                        year,
                        dstRule.DaylightDate,
                        TziUtils.GetDaylightTimeUtcOffset(dstRule),
                        TziUtils.GetStandardTimeUtcOffset(dstRule),
                        TimeZoneMode.DaylightTime
                    )
                };
            }
        }

        private static TimeZonePeriod GenerateTimeZonePeriod
        (
            int year,
            SystemTime startDate,
            TimeDuration periodUtcOffset,
            TimeDuration previousPeriodUtcOffset,
            TimeZoneMode timeZoneMode
        )
        {
            var localTransitionTime = DetermineLocalTransitionTime(year, startDate);

            return new TimeZonePeriod
            (
                year,
                localTransitionTime,
                periodUtcOffset - previousPeriodUtcOffset,
                CalculateUtcPeriodStart(localTransitionTime, previousPeriodUtcOffset),
                periodUtcOffset,
                timeZoneMode
            );
        }

        private static DateTimeValues DetermineLocalTransitionTime(int year, SystemTime startDate)
        {
            var localTransitionTime = new UtcDateTime
            (
                year,
                startDate.Month,
                1,
                startDate.Hour,
                startDate.Minute,
                startDate.Second,
                startDate.Milliseconds
            );

            localTransitionTime += CalculateDayOffset((Weekday) startDate.DayOfWeek, localTransitionTime.Weekday);

            for (short weekIndex = 1; weekIndex < startDate.Day; weekIndex++)
            {
                var advancedDate = localTransitionTime + TimeDuration.OneWeek;
                if (advancedDate.Month == localTransitionTime.Month)
                    localTransitionTime = advancedDate;
                else
                    break;
            }

            return localTransitionTime.DateTimeValues;
        }

        private static UtcDateTime CalculateUtcPeriodStart(DateTimeValues localTransitionTime, TimeDuration previousUtcOffset)
        {
            return new UtcDateTime
            (
                localTransitionTime.Year,
                localTransitionTime.Month,
                localTransitionTime.Day,
                localTransitionTime.Hour,
                localTransitionTime.Minute,
                localTransitionTime.Second,
                localTransitionTime.Millisecond
            ) - previousUtcOffset;
        }

        private static TimeDuration CalculateDayOffset(Weekday targetWeekday, Weekday currentWeekday)
        {
            return TimeDuration.FromDays
            (
                (targetWeekday - currentWeekday + TimeConverter.DaysPerWeek)
                    % TimeConverter.DaysPerWeek
            );
        }

        public override string ToString()
        {
            return $"{TimeZoneMode}, {UtcOffset}";
        }
    }
}
