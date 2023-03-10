using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        /// <summary>
        /// Processing the VariablesUpdateAtServer packet.
        /// Replies with PacketId.VariablesUpdateAtServer or PacketId.ErrorResponseFromServer.
        /// </summary>
        /// <param name="clientRef">Sender's information / address.</param>
        /// <param name="data">The data packet.</param>
        /// <param name="offset">Offset from where packet parsing is to be started.</param>
        private void ProcessPacket_VariablesUpdateAtServer(string clientRef, byte[] data, ref int offset)
        {
            /*
             * Packet Format:
             * --------------
             * 
             *     Client -> Server
             *     ----------------
             *     ->
             *     
             *     Server -> Client
             *     ----------------
             *     ->
             *     
             */
        }
    }
}
