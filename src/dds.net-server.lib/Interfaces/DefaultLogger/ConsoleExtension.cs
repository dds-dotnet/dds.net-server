namespace DDS.Net.Server.Interfaces.DefaultLogger
{
    public static class ConsoleExtension
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
}
