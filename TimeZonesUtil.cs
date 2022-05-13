
namespace TimeZones
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Win32;

    static class TimeZonesUtil
    {
        private static readonly string TimeZonesSubKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones";

        public static List<string> GetTimeZones()
        {
            List<string> timeZonesUTC = new List<string>();
            List<string> timeZonesUTCPlus = new List<string>();
            List<string> timeZonesUTCMinus = new List<string>();

            try
            {
                // Retrieve the sub key of time zones as readonly.
                RegistryKey key = Registry.LocalMachine.OpenSubKey(TimeZonesSubKey);
                if(key == null)
                {
                    Console.WriteLine($"Failed to open the subkey {TimeZonesSubKey}");
                    return null;
                }

                // Go thru all the sub keys to collect their "Display" key value.
                foreach(var subKeyName in key.GetSubKeyNames())
                {
                    var value = key.OpenSubKey(subKeyName)?.GetValue("Display") as string;

                    if(String.IsNullOrEmpty(value))
                        continue;

                    if(value.StartsWith("(UTC)"))
                        timeZonesUTC.Add(value);
                    else if(value.StartsWith("(UTC+"))
                        timeZonesUTCPlus.Add(value);
                    else if(value.StartsWith("(UTC-"))
                        timeZonesUTCMinus.Add(value);
                }

                timeZonesUTCMinus.Sort((first, second) => String.Compare(second, first, comparisonType: StringComparison.OrdinalIgnoreCase));
                timeZonesUTC.Sort((first, second) => String.Compare(first, second, comparisonType: StringComparison.OrdinalIgnoreCase));
                timeZonesUTCPlus.Sort((first, second) => String.Compare(first, second, comparisonType: StringComparison.OrdinalIgnoreCase));

                timeZonesUTCMinus.AddRange(timeZonesUTC);
                timeZonesUTCMinus.AddRange(timeZonesUTCPlus);

                return timeZonesUTCMinus;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to get the time zones, error: {ex}");
            }
            return null;
        }
    }
}
