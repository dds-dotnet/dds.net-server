using DDS.Net.Server.ConsoleApp.Configuration;
using DDS.Net.Server.Entities;
using DDS.Net.Server.Interfaces;
using DDS.Net.Server.Interfaces.DefaultLogger;
using DDS.Net.Server.PublicExtensions;
using DDS.Net.Server.PublicHelpers;

namespace DDS.Net.Server.ConsoleApp
{
    internal class Program
    {
        private static ConsoleLogger logger = new(LogLevel.Information);

        private static DdsServer server = null!;

        static void Main(string[] args)
        {
            Console.Title = $"DDS.Net Server";

            "DDS.Net Server".WriteLine(ConsoleColor.DarkCyan);
            "--------------".WriteLine(ConsoleColor.DarkCyan);

            (bool isEnabled, ServerConfiguration? config) =
                ConfigurationProvider
                  .GetServerConfiguration(
                    AppConstants.SERVER_01_CONFIG_FILENAME, logger);

            if (isEnabled && config != null)
            {
                server = new DdsServer(
                    config,
                    ConfigurationProvider
                      .GetVariablesConfiguration(
                        AppConstants.VARIABLES_CONFIG_FILENAME, logger));

                server.Start();
            }
            else
            {
                server = null!;
                logger.Error("Failed to initialize the server with configuration.");
                logger.Error("The server is either not enabled, or its configuration cannot be read.");
            }

            ConsoleHelpers.WaitForKey("Press ESC to exit.", ConsoleKey.Escape);

            server?.Stop();
        }
    }
}