using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        /// <summary>
        /// Processing the HandShake packet.
        /// Replies with PacketId.HandShake or PacketId.ErrorResponseFromServer.
        /// </summary>
        /// <param name="clientRef">Sender's information / address.</param>
        /// <param name="data">The data packet.</param>
        /// <param name="offset">Offset from where packet parsing is to be started.</param>
        private void ProcessPacket_HandShake(string clientRef, byte[] data, ref int offset)
        {
            /*
             * Packet Format:
             * --------------
             * 
             *     Client -> Server
             *     ----------------
             *     [String: Client Application Name]
             *     [String: Client Library Version]
             *     
             *     Server -> Client
             *     ----------------
             *     [String: Server Application Name]
             *     [String: Server Library Version]
             *     
             */
        }
    }
}
