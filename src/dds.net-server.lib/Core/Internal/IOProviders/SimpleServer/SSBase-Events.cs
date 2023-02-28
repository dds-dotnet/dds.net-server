using DDS.Net.Server.Core.Internal.IOProviders.SimpleServer.Types;

namespace DDS.Net.Server.Core.Internal.SimpleServer
{
    internal abstract partial class SSBase
    {
        public event EventHandler<SSStatus>? ServerStatusChanged;
        public event EventHandler<SSPacket>? ClientPacketReceived;
    }
}
