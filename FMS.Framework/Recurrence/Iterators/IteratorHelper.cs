using FMS.Framework.Core;

namespace FMS.Framework.Recurrence.Iterators
{
    internal static class IteratorHelper
    {
        public static void IncrementYear(ref int year)
        {
            year++;
        }

        public static void IncrementMonth(ref int year, ref int month)
        {
            month++;
            if (month > TimeConverter.MonthMaximum)
            {
                month = TimeConverter.MonthMinimum;
                IncrementYear(ref year);
            }
        }

        public static void IncrementWeek(ref int year, ref int month, ref int day)
        {
            for (int dayIndex = 0; dayIndex < TimeConverter.DaysPerWeek; dayIndex++)
                IncrementDay(ref year, ref month, ref day);
        }

        public static void IncrementDay(ref int year, ref int month, ref int day)
        {
            var dayRanges = TimeConverter.GetDayRanges(year, month);

            day++;
            if (day > dayRanges.Maximum)
            {
                IncrementMonth(ref year, ref month);

                dayRanges = TimeConverter.GetDayRanges(year, month);
                day = dayRanges.Minimum;
            }
        }
    }
}
