namespace DDS.Net.Server.Interfaces.DefaultLogger
{
    internal static class ColoredConsoleExtension
    {
        private static Mutex mutex = new Mutex();

        internal static void PrintColoredLine(
            this string message,
            ConsoleColor fgColor = ConsoleColor.White,
            ConsoleColor bgColor = ConsoleColor.Black)
        {
            lock (mutex)
            {
                ConsoleColor beforeFG = Console.ForegroundColor;
                ConsoleColor beforeBG = Console.BackgroundColor;

                Console.ForegroundColor = fgColor;
                Console.BackgroundColor = bgColor;

                Console.WriteLine(message);

                Console.ForegroundColor = beforeFG;
                Console.BackgroundColor = beforeBG;
            }
        }
    }

    public class ConsoleLogger : ILogger
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
