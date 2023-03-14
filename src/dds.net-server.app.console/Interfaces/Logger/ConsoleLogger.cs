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
                ConsoleColor before = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Error: {message}");
                
                Console.ForegroundColor = before;
            }
        }

        public void Info(string message)
        {
            if (_logLevel == LogLevel.Information)
            {
                lock (this)
                {
                    ConsoleColor before = Console.ForegroundColor;

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(message);

                    Console.ForegroundColor = before;
                }
            }
        }

        public void Warning(string message)
        {
            if (_logLevel != LogLevel.Error)
            {
                lock (this)
                {
                    ConsoleColor before = Console.ForegroundColor;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Warning: {message}");

                    Console.ForegroundColor = before;
                }
            }
        }
    }
}
