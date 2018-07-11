using System;
using System.Collections.Generic;
using FMS.Framework.Core;
using FMS.Framework.Exceptions;

namespace FMS.Framework.Recurrence.Rules
{
    internal static class RuleHelper
    {
        public static Date AdvanceToWeekday(Date date, Weekday weekday, bool inclusive = true)
        {
            int dayCount = ((int) weekday - (int) date.Weekday + TimeConverter.DaysPerWeek) % TimeConverter.DaysPerWeek;
            if (!inclusive && dayCount == 0)
                dayCount += TimeConverter.DaysPerWeek;

            return date.AddDays(dayCount);
        }

        public static Date BackupToWeekday(Date date, Weekday weekday, bool inclusive = true)
        {
            int dayCount = ((int) weekday - (int) date.Weekday - TimeConverter.DaysPerWeek) % TimeConverter.DaysPerWeek;
            if (!inclusive && dayCount == 0)
                dayCount -= TimeConverter.DaysPerWeek;

            return date.AddDays(dayCount);
        }

        public static IReadOnlyList<Date> GetWeekdayOccurrences(int year, int month, Weekday weekday)
        {
            var monthStart = new Date(year, month, TimeConverter.DayMinimum);

            var occurrences = new List<Date>();

            var occurrence = AdvanceToWeekday(monthStart, weekday);
            while (occurrence.Month == month)
            {
                occurrences.Add(occurrence);
                occurrence = occurrence.AddDays(TimeConverter.DaysPerWeek);
            }

            return occurrences;
        }

        public static Date ResolveDate(int year, int month, WeekdayOrdinal weekdayOrdinal, Weekday weekday)
        {
            var monthWeekdays = RuleHelper.GetWeekdayOccurrences(year, month, weekday);
            var monthWeekdayCount = monthWeekdays.Count;

            switch (weekdayOrdinal)
            {
                case WeekdayOrdinal.First: return monthWeekdays[0];
                case WeekdayOrdinal.Second: return monthWeekdays[1];
                case WeekdayOrdinal.Third: return monthWeekdays[2];
                case WeekdayOrdinal.Fourth: return monthWeekdays[3];
                case WeekdayOrdinal.Last: return monthWeekdays[monthWeekdays.Count - 1];
            }

            throw new InvalidOperationException();
        }

        public static Exception CreateInvalidRuleException(string parameterName)
        {
            return new InvalidRuleException($"Invalid rule parameter: {parameterName}");
        }
    }
}
