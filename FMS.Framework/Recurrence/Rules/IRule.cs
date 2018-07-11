using FMS.Framework.Core;
using FMS.Framework.Recurrence.Iterators;

namespace FMS.Framework.Recurrence.Rules
{
    public interface IRule
    {
        RecurrenceType RecurrenceType { get; }
        IRecurrenceIterator GetIterator(Date dateStart);
        Date ResolveDate(Date referenceDate);
    }
}
