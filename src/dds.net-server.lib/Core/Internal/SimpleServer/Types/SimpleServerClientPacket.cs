using System.Net;

namespace DDS.Net.Server.Core.Internal.SimpleServer.Types
{
    internal class SimpleServerClientPacket
    {
        public IPEndPoint ClientInfo { get; }
        public byte[] PacketData { get; }

        public SimpleServerClientPacket(IPEndPoint from, byte[] data)
        {
            ClientInfo = from;
            PacketData = data;
        }
    }
}
