using FMS.Framework.Core;
using FMS.Framework.Recurrence.Iterators;

namespace FMS.Framework.Recurrence.Rules
{
    public class MonthlyDateRule : IRule
    {
        public MonthlyDateRule(int day, int monthOrdinal = 1)
        {
            if (day < TimeConverter.DayMinimum)
                throw RuleHelper.CreateInvalidRuleException(nameof(day));

            if (monthOrdinal < 1)
                throw RuleHelper.CreateInvalidRuleException(nameof(monthOrdinal));

            Day = day;
            MonthOrdinal = monthOrdinal;
        }
        
        public int Day { get; }
        public int MonthOrdinal { get; }

        public RecurrenceType RecurrenceType => RecurrenceType.Monthly;
        public IRecurrenceIterator GetIterator(Date dateStart) => new MonthlyIterator(dateStart, MonthOrdinal);

        public Date ResolveDate(Date referenceDate)
        {
            var dayRanges = TimeConverter.GetDayRanges(referenceDate.Year, referenceDate.Month);
            var effectiveDay = Day > dayRanges.Maximum
                ? dayRanges.Maximum
                : Day;

            return new Date(referenceDate.Year, referenceDate.Month, effectiveDay);
        }
    }
}
