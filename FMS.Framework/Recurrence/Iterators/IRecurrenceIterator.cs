using System.Collections.Generic;
using FMS.Framework.Core;

namespace FMS.Framework.Recurrence.Iterators
{
    public interface IRecurrenceIterator
    {
        IEnumerable<Date> ReferenceDates { get; }
    }
}
