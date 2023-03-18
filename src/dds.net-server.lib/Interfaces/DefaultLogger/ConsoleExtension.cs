namespace DDS.Net.Server.Interfaces.DefaultLogger
{
    public static class ConsoleExtension
    {
        private static Mutex mutex = new Mutex();

        /// <summary>
        /// Extension method to output colored text with a line-end on standard console
        /// and restores previously set colors.
        /// </summary>
        /// <param name="message">Text to output.</param>
        /// <param name="fgColor">Text color.</param>
        /// <param name="bgColor">Background color.</param>
        public static void WriteLine(
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
        /// <summary>
        /// Extension method to output colored text without a line-end on standard console
        /// and restores previously set colors.
        /// </summary>
        /// <param name="message">Text to output.</param>
        /// <param name="fgColor">Text color.</param>
        /// <param name="bgColor">Background color.</param>
        public static void Write(
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
