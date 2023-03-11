using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        /// <summary>
        /// Processing the <c cref="PacketId.VariablesUpdateAtServer">VariablesUpdateAtServer</c> packet.
        /// Replies with
        /// <c cref="PacketId.VariablesUpdateAtServer">VariablesUpdateAtServer</c> or
        /// <c cref="PacketId.ErrorResponseFromServer">ErrorResponseFromServer</c>.
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
             *     Client -> Server : PacketId.VariablesUpdateAtServer
             *     ---------------------------------------------------
             *     [UnsignedWord: Variable Id]
             *     [VariableType: Main Type of the Variable]
             *     [Type (Depends on VariableType): Sub-type of the Variable]
             *     [Any: Value of the Variable]
             *     ...
             *     ...
             *     
             *     Server -> Client : PacketId.VariablesUpdateAtServer
             *     ---------------------------------------------------
             *     ->
             *     
             *     Server -> Client : PacketId.ErrorResponseFromServer
             *     ---------------------------------------------------
             *     [String: Error Message]
             *     
             */
        }
    }
}
