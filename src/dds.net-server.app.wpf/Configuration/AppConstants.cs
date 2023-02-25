using System;

namespace DDS.Net.Server.WpfApp.Configuration
{
    internal static class AppConstants
    {
        static private readonly DateTime _TIMESTAMP = DateTime.Now;
        static public readonly string _TIMESTAMP_TEXT =
            $"{_TIMESTAMP.Year}_{_TIMESTAMP.Month,02:00}_{_TIMESTAMP.Day,02:00}_" +
            $"{_TIMESTAMP.Hour,02:00}_{_TIMESTAMP.Minute,02:00}_{_TIMESTAMP.Second,02:00}";

        static public readonly string LOG_FILENAME = $"Log/log-{_TIMESTAMP_TEXT}.txt";
        static public readonly string SERVER_01_CONFIG_FILENAME = "Configuration/server-01.ini";
    }
}
