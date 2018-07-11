using System.Collections.Generic;
using System.Linq;

namespace FMS.Framework.Utils
{
    public static class EnumUtils
    {
        public static bool IsValidEnumValue<TEnum>(TEnum enumValue)
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum)
                return false;

            return enumType.GetEnumValues().Cast<TEnum>().Contains(enumValue);
        }

        public static bool AreValidEnumValues<TEnum>(IEnumerable<TEnum> enumValues)
        {
            var enumType = typeof(TEnum);
            if (!enumType.IsEnum)
                return false;

            var validEnumValues = enumType.GetEnumValues().Cast<TEnum>().ToList();
            return enumValues.All(enumValue => validEnumValues.Contains(enumValue));
        }
    }
}
