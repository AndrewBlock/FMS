using System;
using System.Collections.Generic;
using FMS.Framework.Core;

namespace FMS.Framework.Recurrence.Iterators
{
    internal class MonthlyIterator : IRecurrenceIterator
    {
        public MonthlyIterator(Date dateStart, int monthOrdinal)
        {
            if (dateStart == null)
                throw new ArgumentException(nameof(dateStart));

            if (monthOrdinal < 1)
                throw new ArgumentException(nameof(monthOrdinal));

            DateStart = dateStart;
            MonthOrdinal = monthOrdinal;
        }

        public Date DateStart { get; }
        public int MonthOrdinal { get; }

        public IEnumerable<Date> ReferenceDates
        {
            get
            {
                var year = DateStart.Year;
                var month = DateStart.Month;

                while (true)
                {
                    yield return new Date(year, month, TimeConverter.DayMinimum);

                    for (int ordinal = 0; ordinal < MonthOrdinal; ordinal++)
                        IteratorHelper.IncrementMonth(ref year, ref month);
                }
            }
        }
    }
}
