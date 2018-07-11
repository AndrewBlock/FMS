using FMS.Framework.Core;
using FMS.Framework.Recurrence.Iterators;
using FMS.Framework.Utils;

namespace FMS.Framework.Recurrence.Rules
{
    public class YearlyMonthDayRule : IRule
    {
        public YearlyMonthDayRule(int month, WeekdayOrdinal weekdayOrdinal, Weekday weekday, int yearOrdinal = 1)
        {
            var monthRanges = TimeConverter.GetMonthRanges();
            if (month < monthRanges.Minimum || month > monthRanges.Maximum)
                throw RuleHelper.CreateInvalidRuleException(nameof(month));

            if (!EnumUtils.IsValidEnumValue(weekdayOrdinal))
                throw RuleHelper.CreateInvalidRuleException(nameof(weekdayOrdinal));

            if (!EnumUtils.IsValidEnumValue(weekday))
                throw RuleHelper.CreateInvalidRuleException(nameof(weekday));

            if (yearOrdinal < 1)
                throw RuleHelper.CreateInvalidRuleException(nameof(yearOrdinal));

            Month = month;
            WeekdayOrdinal = weekdayOrdinal;
            Weekday = weekday;
            YearOrdinal = yearOrdinal;
        }

        public int Month { get; }
        public WeekdayOrdinal WeekdayOrdinal { get; }
        public Weekday Weekday { get; }
        public int YearOrdinal { get; }

        public RecurrenceType RecurrenceType => RecurrenceType.Yearly;
        public IRecurrenceIterator GetIterator(Date dateStart) => new YearlyIterator(dateStart, YearOrdinal);

        public Date ResolveDate(Date referenceDate)
        {
            return RuleHelper.ResolveDate(referenceDate.Year, Month, WeekdayOrdinal, Weekday);
        }
    }
}
