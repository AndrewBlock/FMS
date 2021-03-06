﻿using System.Collections.Generic;
using System.Linq;
using FMS.Framework.Core;
using FMS.Framework.Recurrence;
using FMS.Framework.Recurrence.Rules;
using FMS.Framework.Recurrence.Transforms;
using FMS.Framework.Scheduling.Holidays;

namespace FMS.Framework.ConsoleTest
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                MainOperation();
                return 0;
            }
            catch (System.Exception exception)
            {
                return 1;
            }
        }

        private static void MainOperation()
        {
            var pacificTimeZone = TimeZone.LookupTimeZoneByIdentifier("Pacific Standard Time");
            var mountainTimeZone = TimeZone.LookupTimeZoneByIdentifier("Mountain Standard Time");
            var saskatchewanTimeZone = TimeZone.LookupTimeZoneByIdentifier("Canada Central Standard Time");
            var newfoundlandTimeZone = TimeZone.LookupTimeZoneByIdentifier("Newfoundland Standard Time");
            var arizonaTimeZone = TimeZone.LookupTimeZoneByIdentifier("US Mountain Standard Time");
            var tasmaniaTimeZone = TimeZone.LookupTimeZoneByIdentifier("Tasmania Standard Time");

            var rules = tasmaniaTimeZone.GetTimeZoneModeChangesForYear(2018);

            var utcTime = UtcDateTime.Now;
            var pacificTime = new LocalDateTime(utcTime, pacificTimeZone);
            var mountainTime = new LocalDateTime(utcTime, mountainTimeZone);
            var saskatchewanTime = new LocalDateTime(utcTime, saskatchewanTimeZone);
            var newfoundlandTime = new LocalDateTime(utcTime, newfoundlandTimeZone);
            var arizonaTime = new LocalDateTime(utcTime, arizonaTimeZone);
            var tasmaniaTime = new LocalDateTime(utcTime, tasmaniaTimeZone);

            var saskatchewanDisplay = saskatchewanTime.ToString();

            var utcTime1 = new UtcDateTime(2017, 3, 12, 9, 59, 0, 0);
            var localTime1 = new LocalDateTime(utcTime1, pacificTimeZone);

            var utcTime2 = new UtcDateTime(2017, 3, 12, 10, 0, 0, 0);
            var localTime2 = new LocalDateTime(utcTime2, pacificTimeZone);

            var utcTime3 = new UtcDateTime(2017, 3, 12, 10, 1, 0, 0);
            var localTime3 = new LocalDateTime(utcTime3, pacificTimeZone);

            var standardSpringLocalTime = new LocalDateTime(2018, 3, 11, 1, 59, 59, 999, pacificTimeZone, TimeZoneMode.StandardTime);
            var daylightSpringLocalTime = new LocalDateTime(2018, 3, 11, 3, 0, 0, 0, pacificTimeZone, TimeZoneMode.DaylightTime);
            //var nonExistentSpringLocalTime = new LocalDateTime(2018, 3, 11, 2, 59, 59, 999, pacificTimeZone);

            var springAheadTime = standardSpringLocalTime + new TimeDuration(0, 0, 0, 0, 1);
            var fallBackTime = daylightSpringLocalTime - new TimeDuration(0, 0, 0, 0, 1);

            var daylightFallLocalTime = new LocalDateTime(2018, 11, 4, 1, 30, 5, 444, pacificTimeZone, TimeZoneMode.DaylightTime);
            var standardFallLocalTime = new LocalDateTime(2018, 11, 4, 1, 30, 5, 444, pacificTimeZone, TimeZoneMode.StandardTime);

            var otherDate = new Date(2018, 7, 4);

            var dateTime1 = new System.DateTime(2011, 5, 23);
            var dateTime2 = new System.DateTime(2015, 1, 22);
            var dtResult = dateTime1.CompareTo(dateTime2);

            var date1 = new Date(2011, 5, 23);
            var date2 = new Date(2015, 1, 22);
            var result = date1.CompareTo(date2);

            var bcHolidayRules = new Holiday []
            {
                new Holiday
                (
                    "New Year's Day",
                    HolidayType.Statuatory,
                    new YearlyMonthDateRule(1, 1),
                    HolidayObservance.NextBusinessDay
                ),
                new Holiday
                (
                    "BC Family Day",
                    HolidayType.Statuatory,
                    new List<(int Year, IRule rule)>
                    {
                        (2013, new YearlyMonthDayRule(2, WeekdayOrdinal.Second, Weekday.Monday)),
                        (2019, new YearlyMonthDayRule(2, WeekdayOrdinal.Third, Weekday.Monday)),
                    },
                    HolidayObservance.OriginalDate
                ),
                new Holiday
                (
                    "Good Friday",
                    HolidayType.Statuatory,
                    new BackupToWeekdayRule
                    (
                        new YearlyEasterRule(),
                        Weekday.Friday,
                        false
                    ),
                    HolidayObservance.OriginalDate
                ),
                new Holiday
                (
                    "Easter Sunday",
                    HolidayType.Cultural,
                    new YearlyEasterRule(),
                    HolidayObservance.OriginalDate
                ),
                new Holiday
                (
                    "Mother's Day",
                    HolidayType.Cultural,
                    new YearlyMonthDayRule(5, WeekdayOrdinal.Second, Weekday.Sunday),
                    HolidayObservance.OriginalDate
                ),
                new Holiday
                (
                    "Victoria Day",
                    HolidayType.Statuatory,
                    new BackupToWeekdayRule
                    (
                        new YearlyMonthDateRule(5, 25),
                        Weekday.Monday,
                        false
                    ),
                    HolidayObservance.OriginalDate
                ),
                new Holiday
                (
                    "Father's Day",
                    HolidayType.Cultural,
                    new YearlyMonthDayRule(6, WeekdayOrdinal.Third, Weekday.Sunday),
                    HolidayObservance.OriginalDate
                ),
                new Holiday
                (
                    "Canada Day",
                    HolidayType.Statuatory,
                    new YearlyMonthDateRule(7, 1),
                    HolidayObservance.NextBusinessDay
                ),
                new Holiday
                (
                    "Civic Holiday",
                    HolidayType.Statuatory,
                    new YearlyMonthDayRule(8, WeekdayOrdinal.First, Weekday.Monday),
                    HolidayObservance.OriginalDate
                ),
                new Holiday
                (
                    "Labour Day",
                    HolidayType.Statuatory,
                    new YearlyMonthDayRule(9, WeekdayOrdinal.First, Weekday.Monday),
                    HolidayObservance.OriginalDate
                ),
                new Holiday
                (
                    "Thanksgiving Monday",
                    HolidayType.Statuatory,
                    new YearlyMonthDayRule(10, WeekdayOrdinal.Second, Weekday.Monday),
                    HolidayObservance.OriginalDate
                ),
                new Holiday
                (
                    "Remembrance Day",
                    HolidayType.Statuatory,
                    new YearlyMonthDateRule(11, 11),
                    HolidayObservance.NextBusinessDay
                ),
                new Holiday
                (
                    "Christmas Day",
                    HolidayType.Statuatory,
                    new YearlyMonthDateRule(12, 25),
                    HolidayObservance.NextBusinessDay
                ),
                new Holiday
                (
                    "Boxing Day",
                    HolidayType.Statuatory,
                    new YearlyMonthDateRule(12, 26),
                    HolidayObservance.NextBusinessDay
                )
            };

            var bcHolidays = bcHolidayRules
                .Select(holiday => $"{holiday.Name}: {holiday.ResolveDate(2020):dddd, MMMM d, yyyy}")
                .ToList();

            var recurrencePattern = new RecurrencePattern
            (
                new Date(2018, 7, 30),
                new WeeklyWeekdayRule
                (
                    new Weekday[]
                    {
                        Weekday.Monday,
                        Weekday.Wednesday,
                        Weekday.Friday
                    },
                    1
                ),
                new RecurrenceEndDate(new Date(2018, 11, 9))
            );

            var recurrenceDates = recurrencePattern.ResolveOccurrences
            (
                new Date(2018, 1, 1),
                new Date(2049, 12, 31)
            );
        }
    }
}
