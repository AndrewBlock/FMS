using FMS.Framework.Core;

namespace FMS.Framework.Recurrence
{
    public class RecurrenceEndDate : RecurrenceEnd
    {
        public RecurrenceEndDate(Date endDate)
        {
            EndDate = endDate;
        }

        public Date EndDate { get; }
    }
}
