using FMS.Framework.Core;
using FMS.Framework.Recurrence.Iterators;
using FMS.Framework.Utils;

namespace FMS.Framework.Recurrence.Rules
{
    public class MonthlyDayRule : IRule
    {
        public MonthlyDayRule(WeekdayOrdinal weekdayOrdinal, Weekday weekday, int monthOrdinal = 1)
        {
            if (!EnumUtils.IsValidEnumValue(weekdayOrdinal))
                throw RuleHelper.CreateInvalidRuleException(nameof(weekdayOrdinal));

            if (!EnumUtils.IsValidEnumValue(weekday))
                throw RuleHelper.CreateInvalidRuleException(nameof(weekday));

            if (monthOrdinal < 1)
                throw RuleHelper.CreateInvalidRuleException(nameof(monthOrdinal));

            WeekdayOrdinal = weekdayOrdinal;
            Weekday = weekday;
            MonthOrdinal = monthOrdinal;
        }
        
        public WeekdayOrdinal WeekdayOrdinal { get; }
        public Weekday Weekday { get; }
        public int MonthOrdinal { get; }

        public RecurrenceType RecurrenceType => RecurrenceType.Monthly;
        public IRecurrenceIterator GetIterator(Date dateStart) => new MonthlyIterator(dateStart, MonthOrdinal);

        public Date ResolveDate(Date referenceDate)
        {
            return RuleHelper.ResolveDate(referenceDate.Year, referenceDate.Month, WeekdayOrdinal, Weekday);
        }
    }
}
