using System;
using System.Collections.Generic;
using FMS.Framework.Core;

namespace FMS.Framework.Recurrence.Iterators
{
    internal class DailyIterator : IRecurrenceIterator
    {
        public DailyIterator(Date dateStart, int dayOrdinal)
        {
            if (dateStart == null)
                throw new ArgumentException(nameof(dateStart));

            if (dayOrdinal < 1)
                throw new ArgumentException(nameof(dayOrdinal));

            DateStart = dateStart;
            DayOrdinal = dayOrdinal;
        }

        public Date DateStart { get; }
        public int DayOrdinal { get; }

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

                    for (int ordinal = 0; ordinal < DayOrdinal; ordinal++)
                        IteratorHelper.IncrementDay(ref year, ref month, ref day);
                }
            }
        }
    }
}
