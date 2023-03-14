using DDS.Net.Server.Interfaces;

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
                $"Error: {message}".PrintColoredLine(ConsoleColor.Magenta);
            }
        }

        public void Info(string message)
        {
            if (_logLevel == LogLevel.Information)
            {
                lock (this)
                {
                    message.PrintColoredLine(ConsoleColor.DarkGray);
                }
            }
        }

        public void Warning(string message)
        {
            if (_logLevel != LogLevel.Error)
            {
                lock (this)
                {
                    $"Warning: {message}".PrintColoredLine(ConsoleColor.Yellow);
                }
            }
        }
    }
}
