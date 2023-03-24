using DDS.Net.Server.PublicExtensions;

namespace DDS.Net.Server.PublicHelpers
{
    /// <summary>
    /// Provides helper methods for working with standard console.
    /// </summary>
    public class ConsoleHelpers
    {
        /// <summary>
        /// Prints a message and waits for specified key to be pressed.
        /// </summary>
        /// <param name="message">Message to print on console before waiting for key press.</param>
        /// <param name="waitKey">Required key that needs to be pressed.</param>
        /// <param name="messageForegroundColor">Optional - Foreground color for the message.</param>
        /// <param name="messageBackgroundColor">Optional - Background color for the message.</param>
        /// <param name="breakTime">Optional - time in milliseconds between consecutive key press checks.</param>
        public static void WaitForKey(
            string message,
            ConsoleKey waitKey,
            ConsoleColor messageForegroundColor = ConsoleColor.White,
            ConsoleColor messageBackgroundColor = ConsoleColor.Black,
            int breakTime = 100)
        {
            message.WriteLine(messageForegroundColor, messageBackgroundColor);

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyPressed = Console.ReadKey(true);

                    if (keyPressed.Key == waitKey)
                    {
                        break;
                    }
                }
                
                Thread.Sleep(breakTime);
            }
        }
    }
}
