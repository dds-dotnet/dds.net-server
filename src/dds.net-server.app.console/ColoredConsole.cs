namespace DDS.Net.Server.ConsoleApp
{
    internal static class ColoredConsole
    {
        internal static void PrintColoredLine(
            this string message,
            ConsoleColor fgColor = ConsoleColor.White,
            ConsoleColor bgColor = ConsoleColor.Black)
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
