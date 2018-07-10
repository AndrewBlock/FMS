using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using FMS.Framework.Extensions;
using FMS.Framework.Interop;

namespace FMS.Framework.Core
{
    public partial class TimeZone
    {
        private const string DisplayValueName = @"Display";
        private const string StandardTimeValueName = @"Std";
        private const string DaylightTimeValueName = @"Dlt";
        private const string TziValueName = @"TZI";
        private const string DynamicDstKeyName = @"Dynamic DST";

        private const int DaylightSavingsStartYear = 1916;
        private static readonly UtcDateTime EpochDateTime = UtcDateTime.MinValue;

        private readonly SortedDictionary<int, IReadOnlyList<TimeZonePeriod>> _timeZonePeriodsByYear
            = new SortedDictionary<int, IReadOnlyList<TimeZonePeriod>>();
        private readonly SortedDictionary<long, TimeZonePeriod> _timeZonePeriodsByUtcTicks
            = new SortedDictionary<long, TimeZonePeriod>();

        internal TimeZone(string identifier, RegistryKey timeZoneKey)
        {
            Identifier = identifier;
            DisplayName = TimeZoneUtils.GetStringKeyValue(timeZoneKey, DisplayValueName);
            StandardTimeName = TimeZoneUtils.GetStringKeyValue(timeZoneKey, StandardTimeValueName);
            DaylightTimeName = TimeZoneUtils.GetStringKeyValue(timeZoneKey, DaylightTimeValueName);
            DefaultDstRule = TimeZoneUtils.GetTziValue(timeZoneKey, TziValueName);
            DynamicDstRules = TimeZoneUtils.GetDynamicDstRules(timeZoneKey, DynamicDstKeyName);
        }

        public string Identifier { get; }
        public string DisplayName { get; }
        public string StandardTimeName { get; }
        public string DaylightTimeName { get; }
        private Tzi DefaultDstRule { get; }
        private IReadOnlyDictionary<int, Tzi> DynamicDstRules { get; }

        private bool UsesOnlyStandardTime =>
            DynamicDstRules.Count == 0 && TziUtils.IsCompletelyStandardTime(DefaultDstRule);

        public IReadOnlyList<TimeZonePeriod> GetTimeZoneModeChangesForYear(int year)
        {
            lock (_timeZonePeriodsByYear)
            {
                EnsureReferenceYearsCached(year);
                return GetTimeZonePeriodsForYear(year);
            }
        }

        public TimeZonePeriod GetActiveTimeZonePeriod(UtcDateTime dateTime)
        {
            lock (_timeZonePeriodsByYear)
            {
                EnsureReferenceYearsCached(dateTime.Year);
                return DetermineActiveTimeZonePeriod(dateTime.Ticks);
            }
        }

        public bool UsesDaylightSavingsForYear(int year)
        {
            return GetTimeZoneModeChangesForYear(year).Count == 2;
        }

        public TimeDuration GetStandardUtcOffsetForYear(int year)
        {
            return GetTimeZoneModeChangesForYear(year)
                .FirstOrDefault(period => period.TimeZoneMode == TimeZoneMode.StandardTime)
                ?.UtcOffset;
        }

        public TimeDuration GetDaylightUtcOffsetForYear(int year)
        {
            return GetTimeZoneModeChangesForYear(year)
                .FirstOrDefault(period => period.TimeZoneMode == TimeZoneMode.DaylightTime)
                ?.UtcOffset;
        }

        private void EnsureReferenceYearsCached(int year)
        {
            EnsureEpochYearTimeZonePeriodCached();

            GetReferenceYears(year)
                .ForEach(EnsureYearTimeZonePeriodsCached);
        }

        private void EnsureEpochYearTimeZonePeriodCached()
        {
            if (_timeZonePeriodsByYear.ContainsKey(EpochDateTime.Year))
                return;

            CacheTimeZonePeriod
            (
                EpochDateTime.Year,
                TimeZonePeriod.GetForcedStandardTimeZonePeriod(EpochDateTime, DefaultDstRule)
            );
        }

        private void CacheTimeZonePeriod(int year, TimeZonePeriod timeZonePeriods)
        {
            CacheTimeZonePeriods(year, new List<TimeZonePeriod> { timeZonePeriods });
        }

        private void CacheTimeZonePeriods(int year, IReadOnlyList<TimeZonePeriod> timeZonePeriods)
        {
            _timeZonePeriodsByYear.Add(year, timeZonePeriods);

            timeZonePeriods.ForEach(period =>
                _timeZonePeriodsByUtcTicks.Add(period.UtcPeriodStart.Ticks, period));
        }

        private IEnumerable<int> GetReferenceYears(int centralYear)
        {
            return new List<int>
            {
                centralYear - 1,
                centralYear,
                centralYear + 1
            };
        }

        private void EnsureYearTimeZonePeriodsCached(int year)
        {
            if (UsesOnlyStandardTime)
                return;

            if (_timeZonePeriodsByYear.ContainsKey(year))
                return;

            var timeZonePeriods = TimeZonePeriod.GetTimeZonePeriodsForYear
            (
                year,
                DetermineEffectiveYearDstRule(year),
                DetermineEffectiveYearDstRule(year - 1)
            ).ToList();

            CacheTimeZonePeriods(year, timeZonePeriods);
        }

        private Tzi DetermineEffectiveYearDstRule(int year)
        {
            var effectiveDstRule = DetermineYearDstRule(year);

            return year < DaylightSavingsStartYear
                ? TziUtils.GetForcedStandardTimeRule(effectiveDstRule)
                : effectiveDstRule;
        }

        private Tzi DetermineYearDstRule(int year)
        {
            int? effectiveyear = null;
            foreach (var yearRule in DynamicDstRules.Keys)
            {
                if (!effectiveyear.HasValue || year >= yearRule)
                    effectiveyear = yearRule;
                else
                    break;
            }

            return effectiveyear.HasValue
                ? DynamicDstRules[effectiveyear.Value]
                : DefaultDstRule;
        }

        private TimeZonePeriod DetermineActiveTimeZonePeriod(long utcTicks)
        {
            long? verifiedTimeZonePeriod = null;
            foreach (var periodStart in _timeZonePeriodsByUtcTicks.Keys)
            {
                if (!verifiedTimeZonePeriod.HasValue || utcTicks >= periodStart)
                    verifiedTimeZonePeriod = periodStart;
                else
                    break;
            }

            return verifiedTimeZonePeriod.HasValue
                ? _timeZonePeriodsByUtcTicks[verifiedTimeZonePeriod.Value]
                : null;
        }

        private IReadOnlyList<TimeZonePeriod> GetTimeZonePeriodsForYear(int year)
        {
            return _timeZonePeriodsByYear.TryGetValue(year, out var periods)
                ? periods
                : new List<TimeZonePeriod>();
        }

        public override bool Equals(object obj)
        {
            return obj is TimeZone timeZone
                ? Identifier.Equals(timeZone.Identifier)
                : false;
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}
