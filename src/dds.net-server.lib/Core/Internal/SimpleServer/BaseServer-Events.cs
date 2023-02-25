using DDS.Net.Server.Core.Internal.SimpleServer.Types;

namespace DDS.Net.Server.Core.Internal.SimpleServer
{
    internal abstract partial class BaseServer
    {
        public event EventHandler<SimpleServerStatus>? ServerStatusChanged;
        public event EventHandler<SimpleServerClientPacket>? ClientPacketReceived;
    }
}
