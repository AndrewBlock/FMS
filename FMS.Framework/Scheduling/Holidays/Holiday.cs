using System;
using System.Collections.Generic;
using FMS.Framework.Core;
using FMS.Framework.Recurrence.Rules;
using FMS.Framework.Utils;

namespace FMS.Framework.Scheduling.Holidays
{
    public class Holiday : IHoliday
    {
        public Holiday(string name, HolidayType type, IEnumerable<(int Year, IRule Rule)> yearRuleRanges, HolidayObservance observance)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            if (!EnumUtils.IsValidEnumValue(type))
                throw new ArgumentException(nameof(type));

            if (!EnumUtils.IsValidEnumValue(observance))
                throw new ArgumentException(nameof(observance));

            Name = name;
            Type = type;
            YearRuleRanges = ProcessYearRuleRanges(yearRuleRanges, nameof(yearRuleRanges));
            Observance = observance;
        }

        public Holiday(string name, HolidayType type, IRule rule, HolidayObservance observance)
            : this(name, type, new List<(int Year, IRule Rule)> { (TimeConverter.YearMinimum, rule) }, observance)
        {
        }

        public string Name { get; }
        public HolidayType Type { get; }
        public IReadOnlyDictionary<int, IRule> YearRuleRanges { get; }
        public HolidayObservance Observance { get; }

        private static IReadOnlyDictionary<int, IRule> ProcessYearRuleRanges(IEnumerable<(int Year, IRule Rule)> yearRuleRanges, string parameterName)
        {
            if (yearRuleRanges == null)
                throw new ArgumentException(parameterName);

            var ranges = new SortedDictionary<int, IRule>();

            try
            {
                foreach (var yearRuleRange in yearRuleRanges)
                {
                    if (yearRuleRange.Year < TimeConverter.YearMinimum)
                        throw new ArgumentException(parameterName);

                    if (yearRuleRange.Rule != null && yearRuleRange.Rule.RecurrenceType != Recurrence.RecurrenceType.Yearly)
                        throw new ArgumentException(parameterName);

                    ranges.Add(yearRuleRange.Year, yearRuleRange.Rule);
                }
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(parameterName, exception);
            }

            return ranges;
        }

        public Date ResolveDate(int year)
        {
            int? effectiveYear = null;
            foreach (var yearBoundary in YearRuleRanges.Keys)
            {
                if (year < yearBoundary)
                    continue;

                if (!effectiveYear.HasValue || year >= yearBoundary)
                    effectiveYear = yearBoundary;
                else
                    break;
            }

            if (!effectiveYear.HasValue)
                return null;

            var rule = YearRuleRanges[effectiveYear.Value];
            if (rule == null)
                return null;

            return rule.ResolveDate(new Date(year, TimeConverter.MonthMinimum, TimeConverter.DayMinimum));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
