using DDS.Net.Server.ConsoleApp.Configuration;
using DDS.Net.Server.Entities;
using DDS.Net.Server.Interfaces;
using DDS.Net.Server.Interfaces.DefaultLogger;
using DDS.Net.Server.PublicHelpers;

namespace DDS.Net.Server.ConsoleApp
{
    internal class Program
    {
        private static SplitLogger logger =
                                new(new FileLogger(AppConstants.LOG_FILENAME, LogLevel.Information),
                                    new ConsoleLogger(LogLevel.Information));

        private static DdsServer server = null!;

        static void Main(string[] args)
        {
            Console.Title = $"DDS.Net Server";

            "DDS.Net Server".PrintColoredLine(ConsoleColor.DarkCyan);
            "--------------".PrintColoredLine(ConsoleColor.DarkCyan);

            (bool isEnabled, ServerConfiguration? config) =
                ConfigurationProvider.GetServerConfiguration(AppConstants.SERVER_01_CONFIG_FILENAME, logger);

            if (isEnabled && config != null)
            {
                server = new DdsServer(
                    config,
                    ConfigurationProvider.GetVariablesConfiguration(AppConstants.VARIABLES_CONFIG_FILENAME, logger));

                server.Start();
            }
            else
            {
                server = null!;
                logger.Error("Failed to initialize the server configuration.");
                logger.Error("Server is either not enabled, or its configuration cannot be read.");
            }

            WaitForKey("Press ESC to exit.", ConsoleKey.Escape);

            server?.Stop();
            logger?.Dispose();
        }

        private static void WaitForKey(string message, ConsoleKey waitKey, int breakTime = 100)
        {
            message.PrintColoredLine(ConsoleColor.White);

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