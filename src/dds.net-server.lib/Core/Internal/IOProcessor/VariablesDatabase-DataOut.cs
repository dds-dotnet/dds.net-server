using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Core.Internal.IOProcessor.Types;
using DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable;
using DDS.Net.Server.Entities;
using System.Text;

namespace DDS.Net.Server.Core.Internal.IOProcessor
{
    internal partial class VariablesDatabase
        : SinglePipedConsumer<DataFromClient, DataToClient, VarsDbCommand, VarsDbStatus>
    {
        /// <summary>
        /// Sends server information (<c cref="PacketId.HandShake">HandShake</c>)
        /// to the client.
        /// </summary>
        /// <param name="clientRef">Client's address.</param>
        private void SendServerInformation(string clientRef)
        {
            if (_serverInfo == null)
            {
                _serverInfo = new byte[
                    EncDecMessageHeader.GetMessageHeaderSizeOnBuffer() +
                    PacketId.HandShake.GetSizeOnBuffer() +
                    2 + Encoding.Unicode.GetBytes(VersionInfo.SERVER_NAME).Length +
                    2 + Encoding.Unicode.GetBytes(VersionInfo.SERVER_VERSION).Length];

                int _serverInfoOffset = 0;

                _serverInfo.WriteMessageHeader(ref _serverInfoOffset, _serverInfo.Length - EncDecMessageHeader.GetMessageHeaderSizeOnBuffer());
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
                                EncDecMessageHeader.GetMessageHeaderSizeOnBuffer() +
                                PacketId.ErrorResponseFromServer.GetSizeOnBuffer() +
                                2 + Encoding.Unicode.GetBytes(message).Length];

            int _errorInfoOffset = 0;

            _errorInfo.WriteMessageHeader(ref _errorInfoOffset, _errorInfo.Length - EncDecMessageHeader.GetMessageHeaderSizeOnBuffer());
            _errorInfo.WritePacketId(ref _errorInfoOffset, PacketId.ErrorResponseFromServer);
            _errorInfo.WriteString(ref _errorInfoOffset, message);

            OutputQueue.Enqueue(new DataToClient(clientRef, _serverInfo));
        }

        /// <summary>
        /// Sends variables' registration response to the client.
        /// </summary>
        /// <param name="clientRef">Client's address.</param>
        /// <param name="registeredVariables">Variables (name => id) that are registered for the client.</param>
        /// <param name="unregisteredVariables">Variables (name => id) that are unregistered for the client.</param>
        private void SendVariablesRegistrationResponse(
            string clientRef,
            Dictionary<string, ushort> registeredVariables,
            Dictionary<string, ushort> unregisteredVariables)
        {
            //- 
            //- Calculating required size for response buffer
            //- 

            int sizeRequired = 0;

            foreach (KeyValuePair<string, ushort> varInfo in registeredVariables)
            {
                sizeRequired +=
                    2 + Encoding.Unicode.GetBytes(varInfo.Key).Length + // string size on buffer
                    2 +                                                 // Id size on buffer
                    1;                                                  // Register/unregister boolean size on buffer
            }

            foreach (KeyValuePair<string, ushort> varInfo in unregisteredVariables)
            {
                sizeRequired +=
                    2 + Encoding.Unicode.GetBytes(varInfo.Key).Length + // string size on buffer
                    2 +                                                 // Id size on buffer
                    1;                                                  // Register/unregister boolean size on buffer
            }


            //- 
            //- Filling-in the response buffer
            //- 

            if (sizeRequired > 0)
            {
                byte[] responseBuffer = new byte[
                                              EncDecMessageHeader.GetMessageHeaderSizeOnBuffer() +
                                              PacketId.VariablesRegistration.GetSizeOnBuffer() + sizeRequired];
                int responseBufferOffset = 0;

                responseBuffer.WriteMessageHeader(ref responseBufferOffset, responseBuffer.Length - EncDecMessageHeader.GetMessageHeaderSizeOnBuffer());
                responseBuffer.WritePacketId(ref responseBufferOffset, PacketId.VariablesRegistration);

                foreach (KeyValuePair<string, ushort> varInfo in registeredVariables)
                {
                    responseBuffer.WriteString(ref responseBufferOffset, varInfo.Key);
                    responseBuffer.WriteUnsignedWord(ref responseBufferOffset, varInfo.Value);
                    responseBuffer.WriteBoolean(ref responseBufferOffset, true);
                }

                foreach (KeyValuePair<string, ushort> varInfo in unregisteredVariables)
                {
                    responseBuffer.WriteString(ref responseBufferOffset, varInfo.Key);
                    responseBuffer.WriteUnsignedWord(ref responseBufferOffset, varInfo.Value);
                    responseBuffer.WriteBoolean(ref responseBufferOffset, false);
                }

                OutputQueue.Enqueue(new DataToClient(clientRef, responseBuffer));
            }
        }

        /// <summary>
        /// Sends updated variables to registered clients.
        /// </summary>
        /// <param name="updatedVariables">List of updated variables.</param>
        /// <param name="periodicity">Only select clients that have registered for specified periodicity.</param>
        private void SendUpdatedVariables(
            List<BaseVariable> updatedVariables,
            Periodicity periodicity = Periodicity.OnChange)
        {
            lock (_dbMutex)
            {
                List<BaseVariable> varsToBeSent = new();

                foreach (string clientRef in GetUniqueSubscribersForVariables(updatedVariables))
                {
                    varsToBeSent.Clear();

                    int bufferSize = 0;

                    foreach (BaseVariable v in GetVariablesForSubscriberWithPeriodicity(
                                                        updatedVariables,
                                                        clientRef,
                                                        periodicity))
                    {
                        varsToBeSent.Add(v);
                        bufferSize += v.GetSizeOnBuffer();
                    }

                    if (bufferSize > 0)
                    {
                        bufferSize += EncDecMessageHeader.GetMessageHeaderSizeOnBuffer();
                        bufferSize += PacketId.VariablesUpdateFromServer.GetSizeOnBuffer();
                        bufferSize += periodicity.GetSizeOnBuffer();

                        byte[] buffer = new byte[bufferSize];
                        int bufferOffset = 0;

                        buffer.WriteMessageHeader(ref bufferOffset, bufferSize - EncDecMessageHeader.GetMessageHeaderSizeOnBuffer());
                        buffer.WritePacketId(ref bufferOffset, PacketId.VariablesUpdateFromServer);
                        buffer.WritePeriodicity(ref bufferOffset, periodicity);

                        foreach (BaseVariable v in varsToBeSent)
                        {
                            v.WriteOnBuffer(ref buffer, ref bufferOffset);
                        }

                        OutputQueue.Enqueue(new DataToClient(clientRef, buffer));
                    }
                }
            }
        }

        /// <summary>
        /// Sends variables with selected periodicity to their subscribers.
        /// </summary>
        /// <param name="periodicity"></param>
        private void DoPeriodicUpdateToSendVariables(Periodicity periodicity)
        {
            lock (_dbMutex)
            {
                List<BaseVariable> varsToBeSent = new();

                foreach (string clientRef in GetAllUniqueSubscribers())
                {
                    varsToBeSent.Clear();

                    int bufferSize = 0;

                    foreach (BaseVariable v in GetVariablesForSubscriberWithPeriodicity(clientRef, periodicity))
                    {
                        varsToBeSent.Add(v);
                        bufferSize += v.GetSizeOnBuffer();
                    }

                    if (bufferSize > 0)
                    {
                        bufferSize += EncDecMessageHeader.GetMessageHeaderSizeOnBuffer();
                        bufferSize += PacketId.VariablesUpdateFromServer.GetSizeOnBuffer();
                        bufferSize += periodicity.GetSizeOnBuffer();

                        byte[] buffer = new byte[bufferSize];
                        int bufferOffset = 0;

                        buffer.WriteMessageHeader(ref bufferOffset, bufferSize - EncDecMessageHeader.GetMessageHeaderSizeOnBuffer());
                        buffer.WritePacketId(ref bufferOffset, PacketId.VariablesUpdateFromServer);
                        buffer.WritePeriodicity(ref bufferOffset, periodicity);

                        foreach (BaseVariable v in varsToBeSent)
                        {
                            v.WriteOnBuffer(ref buffer, ref bufferOffset);
                        }

                        OutputQueue.Enqueue(new DataToClient(clientRef, buffer));
                    }
                }
            }
        }
    }
}
