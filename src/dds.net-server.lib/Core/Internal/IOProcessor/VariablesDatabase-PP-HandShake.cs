using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using System.Text;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        byte[] _serverInfo = null;

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
             *     [String: Server Name]
             *     [String: Server Version]
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
            catch (Exception) { }


            //- 
            //- Sending server information
            //- 

            if (_serverInfo == null)
            {
                byte[] sname = Encoding.Unicode.GetBytes(VersionInfo.SERVER_NAME);
                byte[] svers = Encoding.Unicode.GetBytes(VersionInfo.SERVER_VERSION);
                
                _serverInfo = new byte[sname.Length + svers.Length + 4];

                int _serverInfoOffset = 0;

                _serverInfo.WriteString(ref _serverInfoOffset, VersionInfo.SERVER_NAME);
                _serverInfo.WriteString(ref _serverInfoOffset, VersionInfo.SERVER_VERSION);
            }

            OutputQueue.Enqueue(new DataToClient(clientRef, _serverInfo));
        }
    }
}
