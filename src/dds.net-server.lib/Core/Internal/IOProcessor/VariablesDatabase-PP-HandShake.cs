using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Core.Internal.IOProcessor.Types;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        /// <summary>
        /// Buffer to keep server information for clients in <c>PacketId.HandShake</c> packets.
        /// Initialized once in first requirement, and used everytime onwards.
        /// </summary>
        byte[] _serverInfo = null;

        /// <summary>
        /// Processing the <c cref="PacketId.HandShake">HandShake</c> packet.
        /// Replies with
        /// <c cref="PacketId.HandShake">HandShake</c> or
        /// <c cref="PacketId.ErrorResponseFromServer">ErrorResponseFromServer</c>.
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
             *     Client -> Server : PacketId.HandShake
             *     -------------------------------------
             *     [String: Client Application Name]
             *     [String: Client Library Version]
             *     
             *     Server -> Client : PacketId.HandShake
             *     -------------------------------------
             *     [String: Server Name]
             *     [String: Server Version]
             *     
             *     Server -> Client : PacketId.ErrorResponseFromServer
             *     ---------------------------------------------------
             *     [String: Error Message]
             *     
             */

            try
            {
                //- 
                //- Reading client information
                //- 

                string clientApplicationName = data.ReadString(ref offset);
                string clientLibraryVersion = data.ReadString(ref offset);

                logger.Info(
                    $"Client {clientRef} connected: " +
                    $"{clientApplicationName} (Connector version: {clientLibraryVersion})");
            }
            catch (Exception ex)
            {
                SendErrorMessage(clientRef, ex.Message);
                return;
            }

            SendServerInformation(clientRef);
        }
    }
}
