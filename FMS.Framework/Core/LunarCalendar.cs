using System.Collections.Generic;

namespace FMS.Framework.Core
{
    public static class LunarCalendar
    {
        private const int MetonicCyclePeriod = 19;

        private static readonly IDictionary<int, (int Month, int Day)> MetonicCycles = new Dictionary<int, (int Month, int Day)>
        {
            { 1, (4, 14) },
            { 2, (4, 3) },
            { 3, (3, 23) },
            { 4, (4, 11) },
            { 5, (3, 31) },
            { 6, (4, 18) },
            { 7, (4, 8) },
            { 8, (3, 28) },
            { 9, (4, 16) },
            { 10, (4, 5) },
            { 11, (3, 25) },
            { 12, (4, 13) },
            { 13, (4, 2) },
            { 14, (3, 22) },
            { 15, (4, 10) },
            { 16, (3, 30) },
            { 17, (4, 17) },
            { 18, (4, 7) },
            { 19, (3, 27) },
        };

        public static Date GetPaschalFullMoonDateForYear(int year)
        {
            var date = MetonicCycles[(year % MetonicCyclePeriod) + 1];
            return new Date(year, date.Month, date.Day);
        }
    }
}
