# DdsServer

DDS.Net.Server.**DdsServer** is the main entry-point of the library to execute the server for variables' exchange. Its constructor takes following parameters:

```csharp
DdsServer(
    ServerConfiguration config,
    VariablesConfiguration variablesConfig)
```



As a simple use-case, the server can be executed as:

```csharp
using DDS.Net.Server.Entities;
using DDS.Net.Server.Interfaces;
using DDS.Net.Server.Interfaces.DefaultLogger;
using DDS.Net.Server.PublicExtensions;
using DDS.Net.Server.PublicHelpers;

namespace DDS.Net.Server.ConsoleApp
{
    internal class Program
    {
        private static ConsoleLogger logger =
                  new ConsoleLogger(LogLevel.Information);
        private static DdsServer server = null!;

        static void Main(string[] args)
        {
            (bool isEnabled,
             ServerConfiguration? config) =
                ConfigurationProvider
                    .GetServerConfiguration("config.ini", logger);

            if (isEnabled && config != null)
            {
                server = new DdsServer(
                    config,
                    ConfigurationProvider
                        .GetVariablesConfiguration(
                            "vars-config.ini", logger));

                server.Start();
            }
            else
            {
                server = null!;
                logger.Error("Failed to start the server.");
            }

            ConsoleHelpers
                .WaitForKey("Press ESC to exit.", ConsoleKey.Escape);

            server?.Stop();
        }
    }
}
```





