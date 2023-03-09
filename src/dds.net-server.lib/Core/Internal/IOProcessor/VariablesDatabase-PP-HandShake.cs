using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        private void ProcessPacket_HandShake(string clientRef, byte[] data, ref int offset)
        {
            throw new NotImplementedException();
        }
    }
}
