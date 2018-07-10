using System;
using System.Linq;
using System.Globalization;
using System.Windows.Data;
using FMS.Framework.Core;

namespace FMS.Framework.TimeTestApp.Converters
{
    public class TimeZoneRulesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((targetType == typeof(string)) && (value is LocalDateTime dateTime))
            {
                var timeZone = dateTime.TimeZone;
                if (timeZone.UsesDaylightSavingsForYear(dateTime.Year))
                {
                    return string.Join
                    (
                        "\n",
                        timeZone.GetTimeZoneModeChangesForYear(dateTime.Year)
                        .Select(period => $"{GetDisplayString(period.TimeZoneMode)}: {period.LocalTransitionTime.ToString("f", CultureInfo.CurrentUICulture)}")
                    );
                }

                return $"No DST transition for year {dateTime.Year}";
            }

            return null;
        }

        private static string GetDisplayString(TimeZoneMode timeZoneMode)
        {
            return timeZoneMode == TimeZoneMode.DaylightTime
                ? "Daylight Time"
                : "Standard Time";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
