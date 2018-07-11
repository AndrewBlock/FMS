using System;
using System.Collections.Generic;
using System.Linq;
using FMS.Framework.Core;
using FMS.Framework.Recurrence.Iterators;
using FMS.Framework.Utils;

namespace FMS.Framework.Recurrence.Rules
{
    [Serializable]
    public class WeeklyWeekdayRule : IRule
    {
        public WeeklyWeekdayRule(IEnumerable<Weekday> weekdays, int weekOrdinal = 1, Weekday weekStart = Weekday.Sunday)
        {
            Weekdays = new List<Weekday>
            (
                weekdays.Distinct().OrderBy(weekday => weekday)
            );

            if (!EnumUtils.AreValidEnumValues(Weekdays) || Weekdays.Count < 1)
                throw RuleHelper.CreateInvalidRuleException(nameof(weekdays));

            if (weekOrdinal < 1)
                throw RuleHelper.CreateInvalidRuleException(nameof(weekOrdinal));

            if (!EnumUtils.IsValidEnumValue(weekStart))
                throw RuleHelper.CreateInvalidRuleException(nameof(weekStart));

            WeekOrdinal = weekOrdinal;
            WeekStart = weekStart;
        }
        
        public IReadOnlyList<Weekday> Weekdays { get; }
        public int WeekOrdinal { get; }
        public Weekday WeekStart { get; }

        public RecurrenceType RecurrenceType => RecurrenceType.Weekly;
        public IRecurrenceIterator GetIterator(Date dateStart) => new WeeklyIterator(dateStart, WeekOrdinal, WeekStart);

        public Date ResolveDate(Date referenceDate)
        {
            return Weekdays.Contains(referenceDate.Weekday)
                ? referenceDate
                : null;
        }
    }
}
