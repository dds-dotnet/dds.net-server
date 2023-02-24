﻿namespace DDS.Net.Server.Entities
{
    public class ServerConfiguration
    {
        public string ListeningAddressIPv4 { get; }

        public bool EnableTCP { get; }
        public ushort ListeningPortTCP { get; }

        public bool EnableUDP { get; }
        public ushort ListeningPortUDP { get; }

        public ServerConfiguration(
            string listeningIPv4Address,
            bool enableTCP, ushort tcpPort,
            bool enableUDP, ushort udpPort)
        {
            ListeningAddressIPv4 = listeningIPv4Address;
            EnableTCP = enableTCP;
            ListeningPortTCP = tcpPort;
            EnableUDP = enableUDP;
            ListeningPortUDP = udpPort;
        }
    }
}
