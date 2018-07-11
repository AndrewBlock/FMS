using FMS.Framework.Core;
using FMS.Framework.Recurrence.Iterators;

namespace FMS.Framework.Recurrence.Rules
{
    public class DailyRule : IRule
    {
        public DailyRule(int dayOrdinal = 1)
        {
            if (dayOrdinal < 1)
                throw RuleHelper.CreateInvalidRuleException(nameof(dayOrdinal));

            DayOrdinal = dayOrdinal;
        }
        
        public int DayOrdinal { get; }

        public RecurrenceType RecurrenceType => RecurrenceType.Daily;
        public IRecurrenceIterator GetIterator(Date dateStart) => new DailyIterator(dateStart, DayOrdinal);

        public Date ResolveDate(Date referenceDate)
        {
            return referenceDate;
        }
    }
}
