namespace DDS.Net.Server.PublicExtensions
{
    /// <summary>
    /// Provides extension methods for working with standard console.
    /// </summary>
    public static class ConsoleExtensions
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

        /// <summary>
        /// Prints a message and waits for specified key to be pressed.
        /// </summary>
        /// <param name="message">Message to print on console before waiting for key press.</param>
        /// <param name="waitKey">Required key that needs to be pressed.</param>
        /// <param name="breakTime">Optional - time in milliseconds between consecutive key press checks.</param>
        public static void WaitForKey(string message, ConsoleKey waitKey, int breakTime = 100)
        {
            message.WriteLine(ConsoleColor.White);

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyPressed = Console.ReadKey(true);

                    if (keyPressed.Key == waitKey)
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(breakTime);
                    }
                }
            }
        }
    }
}
