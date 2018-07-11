using FMS.Framework.Core;
using FMS.Framework.Recurrence.Iterators;

namespace FMS.Framework.Recurrence.Rules
{
    public class YearlyEasterRule : IRule
    {
        public int YearOrdinal => 1;

        public RecurrenceType RecurrenceType => RecurrenceType.Yearly;
        public IRecurrenceIterator GetIterator(Date dateStart) => new YearlyIterator(dateStart, YearOrdinal);

        public Date ResolveDate(Date referenceDate)
        {
            return RuleHelper.AdvanceToWeekday
            (
                LunarCalendar.GetPaschalFullMoonDateForYear(referenceDate.Year),
                Weekday.Sunday,
                false
            );
        }
    }
}
