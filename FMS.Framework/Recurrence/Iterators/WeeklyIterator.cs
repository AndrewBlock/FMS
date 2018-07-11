using System;
using System.Collections.Generic;
using FMS.Framework.Core;
using FMS.Framework.Utils;

namespace FMS.Framework.Recurrence.Iterators
{
    internal class WeeklyIterator : IRecurrenceIterator
    {
        public WeeklyIterator(Date dateStart, int weekOrdinal, Weekday weekStart)
        {
            if (dateStart == null)
                throw new ArgumentException(nameof(dateStart));

            if (weekOrdinal < 1)
                throw new ArgumentException(nameof(weekOrdinal));

            if (!EnumUtils.IsValidEnumValue(weekStart))
                throw new ArgumentException(nameof(weekStart));

            DateStart = dateStart;
            WeekOrdinal = weekOrdinal;
            WeekStart = weekStart;
        }

        public Date DateStart { get; }
        public int WeekOrdinal { get; }
        public Weekday WeekStart { get; }

        public IEnumerable<Date> ReferenceDates
        {
            get
            {
                var year = DateStart.Year;
                var month = DateStart.Month;
                var day = DateStart.Day;

                while (true)
                {
                    yield return new Date(year, month, day);

                    IteratorHelper.IncrementDay(ref year, ref month, ref day);

                    if (TimeConverter.ResolveWeekday(year, month, day) == WeekStart)
                        for (int ordinal = 1; ordinal < WeekOrdinal; ordinal++)
                            IteratorHelper.IncrementWeek(ref year, ref month, ref day);
                }
            }
        }
    }
}
