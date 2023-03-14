using DDS.Net.Server.Interfaces;

namespace DDS.Net.Server.ConsoleApp.Interfaces.Logger
{
    internal class ConsoleLogger : ILogger
    {
        private readonly LogLevel _logLevel;

        public ConsoleLogger(LogLevel logLevel)
        {
            _logLevel = logLevel;
        }
        public void Error(string message)
        {
            Console.WriteLine($"Error: {message}");
        }

        public void Info(string message)
        {
            if (_logLevel == LogLevel.Information)
            {
                Console.WriteLine(message);
            }
        }

        public void Warning(string message)
        {
            if (_logLevel != LogLevel.Error)
            {
                Console.WriteLine($"Warning: {message}");
            }
        }
    }
}
