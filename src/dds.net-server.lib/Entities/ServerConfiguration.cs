using DDS.Net.Server.Interfaces;

namespace DDS.Net.Server.Entities
{
    public class ServerConfiguration
    {
        public string ListeningAddressIPv4 { get; }

        public bool EnableTCP { get; }
        public ushort ListeningPortTCP { get; }
        public int MaxClientsTCP { get; }

        public bool EnableUDP { get; }
        public ushort ListeningPortUDP { get; }

        public ILogger Logger { get; }

        public ServerConfiguration(
            string listeningIPv4Address,
            bool enableTCP, ushort tcpPort, int tcpMaxClients,
            bool enableUDP, ushort udpPort,
            ILogger logger)
        {
            ListeningAddressIPv4 = listeningIPv4Address;

            EnableTCP = enableTCP;
            ListeningPortTCP = tcpPort;
            MaxClientsTCP = tcpMaxClients;

            EnableUDP = enableUDP;
            ListeningPortUDP = udpPort;

            Logger = logger;
        }
    }
}
