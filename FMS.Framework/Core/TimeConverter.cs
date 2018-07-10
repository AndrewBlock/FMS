using System;

namespace FMS.Framework.Core
{
    internal static class TimeConverter
    {
        public const int YearMinimum = 1;
        public const int YearMaximum = 9999;
        public const int MonthMinimum = 1;
        public const int MonthMaximum = 12;
        public const int DayMinimum = 1;
        public const int HourMinimum = 0;
        public const int HourMaximum = 23;
        public const int MinuteMinimum = 0;
        public const int MinuteMaximum = 59;
        public const int SecondMinimum = 0;
        public const int SecondMaximum = 59;
        public const int MillisecondMinimum = 0;
        public const int MillisecondMaximum = 999;
        public const int NanosecondMinimum = 0;
        public const int NanosecondMaximum = 999;

        public const int DaysPerWeek = 7;
        public const int HoursPerDay = 24;
        public const int MinutesPerHour = 60;
        public const int SecondsPerMinute = 60;
        public const int MillisecondsPerSecond = 1000;
        public const int NanosecondsPerMillisecond = 1000;

        public const long TimeTicksMinimum = 0L;
        public const long TimeTicksMaximum = 3155378975999999999L;
        public const long DurationTicksMinimum = -9223372036854775808L;
        public const long DurationTicksMaximum = 9223372036854775807L;

        private const long TicksPerNanosecond = 10;
        private const long TicksPerMillisecond = TicksPerNanosecond * NanosecondsPerMillisecond;
        private const long TicksPerSecond = TicksPerMillisecond * MillisecondsPerSecond;
        private const long TicksPerMinute = TicksPerSecond * SecondsPerMinute;
        private const long TicksPerHour = TicksPerMinute * MinutesPerHour;
        private const long TicksPerDay = TicksPerHour * HoursPerDay;
        private const long TicksPerWeek = TicksPerDay * DaysPerWeek;

        private const int LeapYearBasicPeriod = 4;
        private const int LeapYearCenturyPeriod = 100;
        private const int LeapYearFullPatternPeriod = 400;

        private const Weekday StartWeekdayAtYearZero = Weekday.Saturday;

        private static readonly int[] StandardMonthDayCounts = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        private static readonly int[] LeapYearMonthDayCounts = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        private static readonly long[] StandardMonthDayTicks;
        private static readonly long[] LeapYearMonthDayTicks;
        private static readonly long[] YearModuloTicks;
        private static readonly long YearOneOffsetTicks;
        private static readonly long LeapYearFullPatternOffsetTicks;


        static TimeConverter()
        {
            StandardMonthDayTicks = CalculateMonthDayTicks(StandardMonthDayCounts);
            LeapYearMonthDayTicks = CalculateMonthDayTicks(LeapYearMonthDayCounts);
            YearModuloTicks = CalculateYearModuloTicks(StandardMonthDayTicks, LeapYearMonthDayTicks);
            YearOneOffsetTicks = YearModuloTicks[0];
            LeapYearFullPatternOffsetTicks = YearModuloTicks[LeapYearFullPatternPeriod - 1];
        }

        private static long[] CalculateMonthDayTicks(int[] monthDayCounts)
        {
            var cumulativeTicks = 0L;
            var tickLengths = new long[MonthMaximum];

            for (var monthIndex = 0; monthIndex < MonthMaximum; monthIndex++)
            {
                tickLengths[monthIndex] = cumulativeTicks += TicksPerDay * monthDayCounts[monthIndex];
            }

            return tickLengths;
        }

        private static long[] CalculateYearModuloTicks(long[] standardMonthDayTicks, long[] leapYearMonthDayTicks)
        {
            var cumulativeTicks = 0L;
            var tickLengths = new long[LeapYearFullPatternPeriod];

            for (var yearModulo = 0; yearModulo < LeapYearFullPatternPeriod; yearModulo++)
            {
                tickLengths[yearModulo] = cumulativeTicks += IsLeapYear(yearModulo)
                        ? leapYearMonthDayTicks[MonthMaximum - 1]
                        : standardMonthDayTicks[MonthMaximum - 1];
            }

            return tickLengths;
        }

        public static void ValidateDateTimeValues(int year, int month, int day, int hour, int minute, int second, int millisecond, int nanosecond)
        {
            ValidateDate(year, month, day);
            ValidateTimeValues(hour, minute, second, millisecond, nanosecond);
        }

        public static DateTimeValues ToDateTimeValues(long timeTicks)
        {
            ValidateTimeTicks(timeTicks);

            timeTicks += YearOneOffsetTicks;

            var weekday = ParseWeekdayTicks(timeTicks);
            var year = ParseYearTicks(ref timeTicks);
            var month = ParseMonthTicks(ref timeTicks, year) + 1;
            var day = ParseValueTicks(ref timeTicks, TicksPerDay) + 1;
            var hour = ParseValueTicks(ref timeTicks, TicksPerHour);
            var minute = ParseValueTicks(ref timeTicks, TicksPerMinute);
            var second = ParseValueTicks(ref timeTicks, TicksPerSecond);
            var millisecond = ParseValueTicks(ref timeTicks, TicksPerMillisecond);
            var nanosecond = ParseValueTicks(ref timeTicks, TicksPerNanosecond);

            return new DateTimeValues(year, month, day, weekday, hour, minute, second, millisecond, nanosecond);
        }

        public static void ValidateDate(int year, int month, int day)
        {
            ValidateValueRange(nameof(year), year, YearMinimum, YearMaximum);
            ValidateValueRange(nameof(month), month, MonthMinimum, MonthMaximum);

            var dayCounts = GetMonthDayCounts(year);
            ValidateValueRange(nameof(day), day, DayMinimum, dayCounts[month - 1]);
        }

        public static void ValidateTimeValues(int hour, int minute, int second, int millisecond, int nanosecond)
        {
            ValidateValueRange(nameof(hour), hour, HourMinimum, HourMaximum);
            ValidateValueRange(nameof(minute), minute, MinuteMinimum, MinuteMaximum);
            ValidateValueRange(nameof(second), second, SecondMinimum, SecondMaximum);
            ValidateValueRange(nameof(millisecond), millisecond, MillisecondMinimum, MillisecondMaximum);
            ValidateValueRange(nameof(nanosecond), nanosecond, NanosecondMinimum, NanosecondMaximum);
        }

        public static (int Minimum, int Maximum) GetYearRanges()
        {
            return (YearMinimum, YearMaximum);
        }

        public static (int Minimum, int Maximum) GetMonthRanges()
        {
            return (MonthMinimum, MonthMaximum);
        }

        public static (int Minimum, int Maximum) GetDayRanges(int year, int month)
        {
            ValidateValueRange(nameof(year), year, YearMinimum, YearMaximum);
            ValidateValueRange(nameof(month), month, MonthMinimum, MonthMaximum);

            var dayCounts = GetMonthDayCounts(year);
            return (DayMinimum, dayCounts[month - 1]);
        }

        public static Date IncrementDays(Date date, int dayCount)
        {
            var year = date.Year;
            var month = date.Month;
            var day = date.Day;

            var monthDayCounts = GetMonthDayCounts(year);

            day += dayCount;

            while (day > monthDayCounts[month - 1])
            {
                day -= monthDayCounts[month - 1];

                month++;
                if (month > MonthMaximum)
                {
                    month = MonthMinimum;

                    year += 1;
                    monthDayCounts = GetMonthDayCounts(year);
                }
            }

            while (day < DayMinimum)
            {
                month--;
                if (month < MonthMinimum)
                {
                    month = MonthMaximum;

                    year -= 1;
                    monthDayCounts = GetMonthDayCounts(year);
                }

                day += monthDayCounts[month - 1];
            }

            return new Date(year, month, day);
        }

        public static void ValidateDurationValues(int days, int hours, int minutes, int seconds, int milliseconds, int nanoseconds)
        {
        }

        public static TimeDurationValues ToTimeDurationValues(long durationTicks)
        {
            ValidateDurationTicks(durationTicks);

            var days = ParseValueTicks(ref durationTicks, TicksPerDay);
            var hours = ParseValueTicks(ref durationTicks, TicksPerHour);
            var minutes = ParseValueTicks(ref durationTicks, TicksPerMinute);
            var seconds = ParseValueTicks(ref durationTicks, TicksPerSecond);
            var milliseconds = ParseValueTicks(ref durationTicks, TicksPerMillisecond);
            var nanoseconds = ParseValueTicks(ref durationTicks, TicksPerNanosecond);

            return new TimeDurationValues(days, hours, minutes, seconds, milliseconds, nanoseconds);
        }

        private static long[] GetMonthDayTicks(int year)
        {
            return IsLeapYear(year) ? LeapYearMonthDayTicks : StandardMonthDayTicks;
        }

        private static int[] GetMonthDayCounts(int year)
        {
            return IsLeapYear(year) ? LeapYearMonthDayCounts : StandardMonthDayCounts;
        }

        private static bool IsLeapYear(int year)
        {
            if (year % LeapYearFullPatternPeriod == 0)
            {
                return true;
            }

            if (year % LeapYearCenturyPeriod == 0)
            {
                return false;
            }

            return (year % LeapYearBasicPeriod == 0);
        }

        private static Weekday ParseWeekdayTicks(long timeTicks)
        {
            return (Weekday)((timeTicks / TicksPerDay + (long) StartWeekdayAtYearZero) % DaysPerWeek);
        }

        private static int ParseYearTicks(ref long ticks)
        {
            int year = (int)(ticks / LeapYearFullPatternOffsetTicks) * LeapYearFullPatternPeriod;
            ticks %= LeapYearFullPatternOffsetTicks;

            year += ParseTickIndex(ref ticks, YearModuloTicks, LeapYearFullPatternPeriod);

            return year;
        }

        private static int ParseMonthTicks(ref long ticks, int year)
        {
            return ParseTickIndex(ref ticks, GetMonthDayTicks(year), MonthMaximum);
        }

        private static int ParseTickIndex(ref long ticks, long[] array, int arrayLength)
        {
            for (var arrayIndex = 0; arrayIndex < arrayLength; arrayIndex++)
            {
                if (ticks < array[arrayIndex])
                {
                    if (arrayIndex > 0)
                    {
                        ticks -= array[arrayIndex - 1];
                    }

                    return arrayIndex;
                }
            }

            throw new InvalidOperationException();
        }

        private static int ParseValueTicks(ref long ticks, long ticksPerValue)
        {
            var value = (int)(ticks / ticksPerValue);
            ticks %= ticksPerValue;

            return value;
        }

        public static void ValidateTimeTicks(long timeTicks)
        {
            ValidateValueRange("Tick", timeTicks, TimeTicksMinimum, TimeTicksMaximum);
        }

        public static long ToTimeTicks(int year, int month, int day, int hour, int minute, int second, int millisecond, int nanosecond)
        {
            ValidateDateTimeValues(year, month, day, hour, minute, second, millisecond, nanosecond);

            var timeTicks = 0L;

            timeTicks += GetYearTicks(year);
            timeTicks += GetMonthTicks(year, month - 1);
            timeTicks += GetValueTicks(day - 1, TicksPerDay);
            timeTicks += GetValueTicks(hour, TicksPerHour);
            timeTicks += GetValueTicks(minute, TicksPerMinute);
            timeTicks += GetValueTicks(second, TicksPerSecond);
            timeTicks += GetValueTicks(millisecond, TicksPerMillisecond);
            timeTicks += GetValueTicks(millisecond, TicksPerNanosecond);

            timeTicks -= YearOneOffsetTicks;

            return timeTicks;
        }

        public static Weekday ResolveWeekday(int year, int month, int day)
        {
            var timeTicks = ToTimeTicks(year, month, day, HourMinimum, MinuteMinimum, SecondMinimum, MillisecondMinimum, NanosecondMinimum);
            timeTicks += YearOneOffsetTicks;
            return ParseWeekdayTicks(timeTicks);
        }

        public static void ValidateDurationTicks(long durationTicks)
        {
            ValidateValueRange("Tick", durationTicks, DurationTicksMinimum, DurationTicksMaximum);
        }

        public static long ToDurationTicks(int days, int hours, int minutes, int seconds, int milliseconds, int nanoseconds)
        {
            ValidateDurationValues(days, hours, minutes, seconds, milliseconds, nanoseconds);

            var durationTicks = 0L;

            durationTicks += GetValueTicks(days, TicksPerDay);
            durationTicks += GetValueTicks(hours, TicksPerHour);
            durationTicks += GetValueTicks(minutes, TicksPerMinute);
            durationTicks += GetValueTicks(seconds, TicksPerSecond);
            durationTicks += GetValueTicks(milliseconds, TicksPerMillisecond);
            durationTicks += GetValueTicks(nanoseconds, TicksPerNanosecond);

            return durationTicks;
        }

        private static long GetYearTicks(int yearIndex)
        {
            var ticks = yearIndex / LeapYearFullPatternPeriod * LeapYearFullPatternOffsetTicks;

            var yearModulo = yearIndex % LeapYearFullPatternPeriod;
            ticks += yearModulo > 0 ? YearModuloTicks[yearModulo - 1] : 0L;

            return ticks;
        }

        private static long GetMonthTicks(int year, int monthIndex)
        {
            return monthIndex > 0 ? GetMonthDayTicks(year)[monthIndex - 1] : 0L;
        }

        private static long GetValueTicks(int valueIndex, long ticksPerValue)
        {
            return valueIndex * ticksPerValue;
        }

        private static void ValidateValueRange(string valueName, long value, long minimumValue, long maximumValue)
        {
            if (value < minimumValue)
            {
                throw new ArgumentException($"{valueName} values cannot be less than {minimumValue}.", valueName);
            }

            if (value > maximumValue)
            {
                throw new ArgumentException($"{valueName} values cannot be greater than {maximumValue}.", valueName);
            }
        }
    }
}
