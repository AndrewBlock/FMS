using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using FMS.Framework.Interop;

namespace FMS.Framework.Core
{
    internal static class TimeZoneUtils
    {
        private const string TimeZoneKeyName = @"TimeZoneKeyName";

        private const string TimeZoneEnumPath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones";
        private const string TimeZoneSettingPath = @"SYSTEM\CurrentControlSet\Control\TimeZoneInformation";

        public static IReadOnlyList<TimeZone> GetTimeZones()
        {
            using (var timeZoneRootEnumKey = Registry.LocalMachine.OpenSubKey(TimeZoneEnumPath, false))
            {
                return timeZoneRootEnumKey.GetSubKeyNames()
                    .Select(timeZoneKeyName =>
                    {
                        using (var timeZoneKey = timeZoneRootEnumKey.OpenSubKey(timeZoneKeyName, false))
                        {
                            return new TimeZone(timeZoneKeyName, timeZoneKey);
                        }
                    })
                    .OrderBy(timeZone => timeZone.DisplayName)
                    .ToList();
            }
        }

        public static string GetDefaultTimeZoneKeyName()
        {
            using (var timeZoneSettingKey = Registry.LocalMachine.OpenSubKey(TimeZoneSettingPath, false))
            {
                return GetStringKeyValue(timeZoneSettingKey, TimeZoneKeyName);
            }
        }

        public static string GetStringKeyValue(RegistryKey registryKey, string valueName)
        {
            return registryKey.GetValue(valueName) as string ?? string.Empty;
        }

        public static Tzi GetTziValue(RegistryKey registryKey, string valueName)
        {
            var managedBuffer = registryKey.GetValue(valueName) as byte[] ?? new byte[] { };
            var nativeBuffer = Marshal.AllocHGlobal(managedBuffer.Length);

            try
            {
                Marshal.Copy(managedBuffer, 0, nativeBuffer, managedBuffer.Length);
                return Marshal.PtrToStructure<Tzi>(nativeBuffer);
            }
            finally
            {
                Marshal.FreeHGlobal(nativeBuffer);
            }
        }

        public static IReadOnlyDictionary<int, Tzi> GetDynamicDstRules(RegistryKey timeZoneKey, string dynamicDstKeyName)
        {
            var dynamicRules = new SortedDictionary<int, Tzi>();

            using (var dynamicDstKey = timeZoneKey.OpenSubKey(dynamicDstKeyName, false))
            {
                if (dynamicDstKey != null)
                {
                    foreach (var valueName in dynamicDstKey.GetValueNames())
                    {
                        if (int.TryParse(valueName, out int year))
                        {
                            dynamicRules.Add(year, GetTziValue(dynamicDstKey, valueName));
                        }
                    }
                }
            }

            return dynamicRules;
        }
    }
}
