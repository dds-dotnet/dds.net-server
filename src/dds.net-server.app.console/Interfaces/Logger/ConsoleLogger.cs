﻿using DDS.Net.Server.Interfaces;

namespace DDS.Net.Server.ConsoleApp.Interfaces.Logger
{
    internal class ConsoleLogger : ILogger
    {
        private readonly LogLevel _logLevel;

        public ConsoleLogger(LogLevel logLevel = LogLevel.Information)
        {
            _logLevel = logLevel;
        }

        public void Error(string message)
        {
            lock (this)
            {
                Console.WriteLine($"Error: {message}");
            }
        }

        public void Info(string message)
        {
            if (_logLevel == LogLevel.Information)
            {
                lock (this)
                {
                    Console.WriteLine(message);
                }
            }
        }

        public void Warning(string message)
        {
            if (_logLevel != LogLevel.Error)
            {
                lock (this)
                {
                    Console.WriteLine($"Warning: {message}");
                }
            }
        }
    }
}
