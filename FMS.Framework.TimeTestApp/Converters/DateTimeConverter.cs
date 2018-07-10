using System;
using System.Globalization;
using System.Windows.Data;
using FMS.Framework.Core;

namespace FMS.Framework.TimeTestApp.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(DateTime))
            {
                return value is UtcDateTime utcDateTime
                    ? new DateTime
                        (
                            utcDateTime.Year,
                            utcDateTime.Month,
                            utcDateTime.Day,
                            utcDateTime.Hour,
                            utcDateTime.Minute,
                            utcDateTime.Second,
                            utcDateTime.Millisecond,
                            DateTimeKind.Utc
                        )
                    : DateTime.MinValue;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(UtcDateTime))
            {
                return value is DateTime dateTime
                    ? new UtcDateTime
                        (
                            dateTime.Year,
                            dateTime.Month,
                            dateTime.Day,
                            dateTime.Hour,
                            dateTime.Minute,
                            dateTime.Second,
                            dateTime.Millisecond
                        )
                    : null;
            }

            return null;
        }
    }
}
