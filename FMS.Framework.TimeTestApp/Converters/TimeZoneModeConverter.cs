using System;
using System.Globalization;
using System.Windows.Data;
using FMS.Framework.Core;

namespace FMS.Framework.TimeTestApp.Converters
{
    public class TimeZoneModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((targetType == typeof(string)) && (value is LocalDateTime dateTime))
            {
                var timeZone = dateTime.TimeZone;
                return dateTime.TimeZonePeriod.TimeZoneMode == TimeZoneMode.DaylightTime
                    ? dateTime.TimeZone.DaylightTimeName
                    : dateTime.TimeZone.StandardTimeName;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
