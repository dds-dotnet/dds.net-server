using System.Net;

namespace DDS.Net.Server.Core.Internal.SimpleServer.Types
{
    internal class SSPacket
    {
        public IPEndPoint ClientInfo { get; }
        public byte[] PacketData { get; }

        public SSPacket(IPEndPoint client, byte[] data)
        {
            ClientInfo = client;
            PacketData = data;
        }
    }
}
