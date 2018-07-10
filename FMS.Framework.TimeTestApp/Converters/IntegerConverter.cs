using System;
using System.Globalization;
using System.Windows.Data;

namespace FMS.Framework.TimeTestApp.Converters
{
    public class IntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((targetType == typeof(string) && value is int intValue))
            {
                return intValue.ToString("D2");
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((targetType == typeof(int) && value is string stringValue))
            {
                return int.Parse(stringValue);
            }

            return null;
        }
    }
}
