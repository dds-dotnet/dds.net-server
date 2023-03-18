using DDS.Net.Server.Interfaces;

namespace DDS.Net.Server.Entities
{
    /// <summary>
    /// Class <c>ServerConfiguration</c> represents configuration for server working.
    /// </summary>
    public class ServerConfiguration
    {
        /// <summary>
        /// The listening IPv4 address for receiving connections and data.
        /// </summary>
        public string ListeningAddressIPv4 { get; }

        /// <summary>
        /// Should the TCP connectivity be enabled or not.
        /// </summary>
        public bool EnableTCP { get; }
        /// <summary>
        /// The local TCP port for receiving connections.
        /// </summary>
        public ushort ListeningPortTCP { get; }
        /// <summary>
        /// The maximum number of TCP clients.
        /// </summary>
        public int MaxClientsTCP { get; }

        /// <summary>
        /// Should the UDP connectivity be enabled or not.
        /// </summary>
        public bool EnableUDP { get; }
        /// <summary>
        /// The local UDP port for receiving data.
        /// </summary>
        public ushort ListeningPortUDP { get; }

        /// <summary>
        /// The implementation of <c>ILogger</c> interface that should
        /// be used by the server.
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Initializes the configuration object.
        /// </summary>
        /// <param name="listeningIPv4Address">The listening IPv4 address for receiving connections and data.</param>
        /// <param name="enableTCP">Should the TCP connectivity be enabled or not.</param>
        /// <param name="tcpPort">The local TCP port for receiving connections.</param>
        /// <param name="tcpMaxClients">The maximum number of TCP clients.</param>
        /// <param name="enableUDP">Should the UDP connectivity be enabled or not.</param>
        /// <param name="udpPort">The local UDP port for receiving data.</param>
        /// <param name="logger">The implementation of <c>ILogger</c> interface that should be used by the server.</param>
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
