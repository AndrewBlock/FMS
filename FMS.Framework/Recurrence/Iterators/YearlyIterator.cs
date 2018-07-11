using System;
using System.Collections.Generic;
using FMS.Framework.Core;

namespace FMS.Framework.Recurrence.Iterators
{
    internal class YearlyIterator : IRecurrenceIterator
    {
        public YearlyIterator(Date dateStart, int yearOrdinal)
        {
            if (dateStart == null)
                throw new ArgumentException(nameof(dateStart));

            if (yearOrdinal < 1)
                throw new ArgumentException(nameof(yearOrdinal));

            DateStart = dateStart;
            YearOrdinal = yearOrdinal;
        }

        public Date DateStart { get; }
        public int YearOrdinal { get; }

        public IEnumerable<Date> ReferenceDates
        {
            get
            {
                var year = DateStart.Year;

                while (true)
                {
                    yield return new Date(year, TimeConverter.MonthMinimum, TimeConverter.DayMinimum);

                    for (int ordinal = 0; ordinal < YearOrdinal; ordinal++)
                        IteratorHelper.IncrementYear(ref year);
                }
            }
        }
    }
}
