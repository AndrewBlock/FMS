using System.Collections.Generic;
using FMS.Framework.Core;
using FMS.Framework.Scheduling.Holidays;

namespace FMS.Framework.Scheduling
{
    public interface IBusinessSchedule
    {
        IReadOnlyList<Weekday> WeekendDays { get; }
        IReadOnlyList<IHoliday> Holidays { get; }

        bool IsBusinessDay(Date date);
    }
}
