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
        byte[] _serverInfo = null;

        /// <summary>
        /// Processing the <c cref="PacketId.HandShake">PacketId.HandShake</c> packet.
        /// Replies with
        /// <c cref="PacketId.HandShake">PacketId.HandShake</c> or
        /// <c cref="PacketId.ErrorResponseFromServer">PacketId.ErrorResponseFromServer</c>.
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

            //- 
            //- Reading client information
            //- 

            try
            {
                string clientApplicationName = data.ReadString(ref offset);
                string clientLibraryVersion = data.ReadString(ref offset);

                logger.Info($"{clientApplicationName} v{clientLibraryVersion} connected from {clientRef}");
            }
            catch (Exception ex)
            {
                byte[] _errorInfo = new byte[
                    PacketId.ErrorResponseFromServer.GetSizeOnBuffer() +
                    2 + ex.Message.Length];

                int _errorInfoOffset = 0;

                _errorInfo.WritePacketId(ref _errorInfoOffset, PacketId.ErrorResponseFromServer);
                _errorInfo.WriteString(ref _errorInfoOffset, ex.Message);

                OutputQueue.Enqueue(new DataToClient(clientRef, _serverInfo));

                return;
            }


            //- 
            //- Sending server information
            //- 

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
    }
}
