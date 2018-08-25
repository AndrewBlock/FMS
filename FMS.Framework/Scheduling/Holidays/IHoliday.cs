using System.Collections.Generic;
using FMS.Framework.Core;
using FMS.Framework.Recurrence.Rules;

namespace FMS.Framework.Scheduling.Holidays
{
    public interface IHoliday
    {
        string Name { get; }
        HolidayType Type { get; }
        IReadOnlyDictionary<int, IRule> YearRuleRanges { get; }
        HolidayObservance Observance { get; }

        Date ResolveDate(int year);
    }
}
