namespace DDS.Net.Server.Interfaces.DefaultLogger
{
    public static class ColoredConsoleExtension
    {
        private static Mutex mutex = new Mutex();

        public static void PrintConsoleLine(
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
        private readonly LogLevel logLevel;

        private readonly ConsoleColor informationTextColor;
        private readonly ConsoleColor informationBackgroundColor;
        private readonly ConsoleColor warningTextColor;
        private readonly ConsoleColor warningBackgroundColor;
        private readonly ConsoleColor errorTextColor;
        private readonly ConsoleColor errorBackgroundColor;

        public ConsoleLogger(
            LogLevel logLevel = LogLevel.Information,

            ConsoleColor informationTextColor = ConsoleColor.DarkGray,
            ConsoleColor informationBackgroundColor = ConsoleColor.Black,
            ConsoleColor warningTextColor = ConsoleColor.Yellow,
            ConsoleColor warningBackgroundColor = ConsoleColor.Black,
            ConsoleColor errorTextColor = ConsoleColor.Magenta,
            ConsoleColor errorBackgroundColor = ConsoleColor.Black)
        {
            this.logLevel = logLevel;

            this.informationTextColor = informationTextColor;
            this.informationBackgroundColor = informationBackgroundColor;
            this.warningTextColor = warningTextColor;
            this.warningBackgroundColor = warningBackgroundColor;
            this.errorTextColor = errorTextColor;
            this.errorBackgroundColor = errorBackgroundColor;
        }

        public void Error(string message)
        {
            lock (this)
            {
                $"Error: {message}".PrintConsoleLine(errorTextColor, errorBackgroundColor);
            }
        }

        public void Info(string message)
        {
            if (logLevel == LogLevel.Information)
            {
                lock (this)
                {
                    message.PrintConsoleLine(informationTextColor, informationBackgroundColor);
                }
            }
        }

        public void Warning(string message)
        {
            if (logLevel != LogLevel.Error)
            {
                lock (this)
                {
                    $"Warning: {message}".PrintConsoleLine(warningTextColor, warningBackgroundColor);
                }
            }
        }
    }
}
