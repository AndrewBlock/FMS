using System.Collections.Generic;
using FMS.Framework.Core;
using FMS.Framework.Recurrence.Rules;

namespace FMS.Framework.Recurrence
{
    public interface IRecurrencePattern
    {
        Date DateStart { get; }
        IRule Rule { get; }
        RecurrenceEnd End { get; }
        RecurrenceType RecurrenceType { get; }

        IReadOnlyList<Date> ResolveOccurrences(Date rangeStart, Date rangeEnd);
    }
}
