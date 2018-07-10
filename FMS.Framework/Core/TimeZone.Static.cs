using System.Collections.Generic;
using System.Linq;

namespace FMS.Framework.Core
{
    public partial class TimeZone
    {
        static TimeZone()
        {
            TimeZones = TimeZoneUtils.GetTimeZones();
            DefaultTimeZone = GetDefaultTimeZone(TimeZones);
        }

        public static IReadOnlyList<TimeZone> TimeZones { get; }
        public static TimeZone DefaultTimeZone { get; }

        public static TimeZone LookupTimeZoneByIdentifier(string identifier)
        {
            return LookupTimeZoneByIdentifier(identifier, TimeZones);
        }

        private static TimeZone LookupTimeZoneByIdentifier(string identifier, IReadOnlyList<TimeZone> timeZones)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                return null;

            return timeZones.FirstOrDefault(timeZone => Equals(timeZone.Identifier, identifier));
        }

        private static TimeZone GetDefaultTimeZone(IEnumerable<TimeZone> allTimeZones)
        {
            return LookupTimeZoneByIdentifier
            (
                TimeZoneUtils.GetDefaultTimeZoneKeyName(),
                TimeZones
            );
        }
    }
}
