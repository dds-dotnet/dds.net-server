using System.Net;

namespace DDS.Net.Server.Core.Internal.SimpleServer.Types
{
    internal enum SimpleServerPacketSendingStatus
    {
        Successful,
        Failed
    }

    internal class SimpleServerPacket
    {
        public IPEndPoint ClientInfo { get; }
        public byte[] PacketData { get; }

        public SimpleServerPacket(IPEndPoint from, byte[] data)
        {
            ClientInfo = from;
            PacketData = data;
        }
    }
}
