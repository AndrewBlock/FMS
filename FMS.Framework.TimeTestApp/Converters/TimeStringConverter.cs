using System;
using System.Globalization;
using System.Windows.Data;
using FMS.Framework.Core;

namespace FMS.Framework.TimeTestApp.Converters
{
    public class TimeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                return value is LocalDateTime dateTime
                    ? dateTime.ToString("T", CultureInfo.CurrentUICulture)
                    : string.Empty;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
