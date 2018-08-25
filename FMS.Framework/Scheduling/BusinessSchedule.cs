using System;
using System.Collections.Generic;
using System.Linq;
using FMS.Framework.Core;
using FMS.Framework.Scheduling.Holidays;
using FMS.Framework.Utils;

namespace FMS.Framework.Scheduling
{
    public class BusinessSchedule : IBusinessSchedule
    {
        public BusinessSchedule(IEnumerable<Weekday> weekendDays, IEnumerable<IHoliday> holidays)
        {
            WeekendDays = new List<Weekday>
            (
                weekendDays.Distinct().OrderBy(weekday => weekday)
            );

            Holidays = new List<IHoliday>(holidays);

            if (!EnumUtils.AreValidEnumValues(WeekendDays) || WeekendDays.Count < 1)
                throw new ArgumentException(nameof(weekendDays));

            if (Holidays.Any(holiday => holiday == null))
                throw new ArgumentException(nameof(weekendDays));
        }

        public IReadOnlyList<Weekday> WeekendDays { get; }
        public IReadOnlyList<IHoliday> Holidays { get; }

        public bool IsBusinessDay(Date date)
        {
            return false;
        }
    }
}
