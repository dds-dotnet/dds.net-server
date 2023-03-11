using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Core.Internal.IOProcessor.Types;
using System.Text;

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

                logger.Info($"{clientApplicationName} v{clientLibraryVersion} connected from {clientRef}");
            }
            catch (Exception ex)
            {
                SendErrorMessage(clientRef, ex.Message);
                return;
            }

            SendServerInformation(clientRef);
        }

        /// <summary>
        /// Sends server information to the client.
        /// </summary>
        /// <param name="clientRef">Client's address.</param>
        private void SendServerInformation(string clientRef)
        {
            if (_serverInfo == null)
            {
                _serverInfo = new byte[
                    PacketId.HandShake.GetSizeOnBuffer() +
                    2 + VersionInfo.SERVER_NAME.Length +
                    2 + VersionInfo.SERVER_VERSION.Length];

                int _serverInfoOffset = 0;

                _serverInfo.WritePacketId(ref _serverInfoOffset, PacketId.HandShake);
                _serverInfo.WriteString(ref _serverInfoOffset, VersionInfo.SERVER_NAME);
                _serverInfo.WriteString(ref _serverInfoOffset, VersionInfo.SERVER_VERSION);
            }

            OutputQueue.Enqueue(new DataToClient(clientRef, _serverInfo));
        }

        /// <summary>
        /// Sends <c cref="PacketId.ErrorResponseFromServer">ErrorResponseFromServer</c>
        /// packet to the specified client.
        /// </summary>
        /// <param name="clientRef">Target client.</param>
        /// <param name="message">Error message.</param>
        private void SendErrorMessage(string clientRef, string message)
        {
            byte[] _errorInfo = new byte[
                                PacketId.ErrorResponseFromServer.GetSizeOnBuffer() +
                                2 + message.Length];

            int _errorInfoOffset = 0;

            _errorInfo.WritePacketId(ref _errorInfoOffset, PacketId.ErrorResponseFromServer);
            _errorInfo.WriteString(ref _errorInfoOffset, message);

            OutputQueue.Enqueue(new DataToClient(clientRef, _serverInfo));
        }
    }
}
