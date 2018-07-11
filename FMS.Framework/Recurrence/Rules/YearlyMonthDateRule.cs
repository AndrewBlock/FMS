using FMS.Framework.Core;
using FMS.Framework.Recurrence.Iterators;

namespace FMS.Framework.Recurrence.Rules
{
    public class YearlyMonthDateRule : IRule
    {
        public YearlyMonthDateRule(int month, int day, int yearOrdinal = 1)
        {
            var monthRanges = TimeConverter.GetMonthRanges();
            if (month < monthRanges.Minimum || month > monthRanges.Maximum)
                throw RuleHelper.CreateInvalidRuleException(nameof(month));

            if (day < TimeConverter.DayMinimum)
                throw RuleHelper.CreateInvalidRuleException(nameof(day));

            if (yearOrdinal < 1)
                throw RuleHelper.CreateInvalidRuleException(nameof(yearOrdinal));

            Month = month;
            Day = day;
            YearOrdinal = yearOrdinal;
        }

        public int Month { get; }
        public int Day { get; }
        public int YearOrdinal { get; }

        public RecurrenceType RecurrenceType => RecurrenceType.Yearly;
        public IRecurrenceIterator GetIterator(Date dateStart) => new YearlyIterator(dateStart, YearOrdinal);

        public Date ResolveDate(Date referenceDate)
        {
            var dayRanges = TimeConverter.GetDayRanges(referenceDate.Year, Month);
            var effectiveDay = Day > dayRanges.Maximum
                ? dayRanges.Maximum
                : Day;

            return new Date(referenceDate.Year, Month, effectiveDay);
        }
    }
}
