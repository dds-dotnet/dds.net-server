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
        /// Processing the <c cref="PacketId.VariablesRegistration">VariablesRegistration</c> packet.
        /// Replies with
        /// <c cref="PacketId.VariablesRegistration">VariablesRegistration</c> or
        /// <c cref="PacketId.ErrorResponseFromServer">ErrorResponseFromServer</c>.
        /// </summary>
        /// <param name="clientRef">Sender's information / address.</param>
        /// <param name="data">The data packet.</param>
        /// <param name="offset">Offset from where packet parsing is to be started.</param>
        private void ProcessPacket_VariablesRegistration(string clientRef, byte[] data, ref int offset)
        {
            /*
             * Packet Format:
             * --------------
             * 
             *     Client -> Server : PacketId.VariablesRegistration
             *     -------------------------------------------------
             *     [String: Variable Name]
             *     [Periodicity: Update Periodicity]
             *     [Boolean: Register or Unregister] - True = Register, False = Unregister
             *     ...
             *     ...
             *     
             *     Server -> Client : PacketId.VariablesRegistration
             *     -------------------------------------------------
             *     [String: Variable Name]
             *     [UnsignedWord: Variable Id]
             *     [Boolean: Register or Unregister] - True = Register, False = Unregister
             *     ...
             *     ...
             *     
             *     Server -> Client : PacketId.ErrorResponseFromServer
             *     ---------------------------------------------------
             *     [String: Error Message]
             *     
             */

            Dictionary<string, ushort> registeredVariables = new();
            Dictionary<string, ushort> unregisteredVariables = new();

            //- 
            //- Registering / unregistering the variables
            //- 

            try
            {
                while (offset < data.Length)
                {
                    (string variableName, Periodicity periodicity, bool isRegister) =
                        ReadVariableRegistrationElements(data, ref offset);

                    if (isRegister)
                    {
                        ushort varId = RegisterVariableClient(clientRef, variableName, periodicity);

                        if (registeredVariables.ContainsKey(variableName))
                        {
                            registeredVariables[variableName] = varId;
                        }
                        else
                        {
                            registeredVariables.Add(variableName, varId);
                        }
                    }
                    else
                    {
                        ushort varId = UnregisterVariableClient(clientRef, variableName, periodicity);

                        if (unregisteredVariables.ContainsKey(variableName))
                        {
                            unregisteredVariables[variableName] = varId;
                        }
                        else
                        {
                            unregisteredVariables.Add(variableName, varId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SendErrorPacket(clientRef, ex.Message);
                return;
            }

            //- 
            //- Sending response to the client
            //- 

            //- Size of buffer

            int sizeRequired = 0;

            foreach (KeyValuePair<string, ushort> varInfo in registeredVariables)
            {
                sizeRequired +=
                    2 + varInfo.Key.Length + // string size on buffer
                    2 +                      // Id size on buffer
                    1;                       // Register/unregister boolean size on buffer
            }

            foreach (KeyValuePair<string, ushort> varInfo in unregisteredVariables)
            {
                sizeRequired +=
                    2 + varInfo.Key.Length + // string size on buffer
                    2 +                      // Id size on buffer
                    1;                       // Register/unregister boolean size on buffer
            }

            byte[] responseBuffer = new byte[sizeRequired];
            int responseBufferOffset = 0;

            //- Filling-in the buffer

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
        /// <summary>
        /// Reads variable registration elements from the given packet.
        /// </summary>
        /// <param name="data">Data buffer.</param>
        /// <param name="offset">Reading offset in the buffer.</param>
        /// <returns>Tuple (
        /// string: variableName,
        /// Periodicity: update periodicity,
        /// bool: is registering
        /// )</returns>
        private static Tuple<string, Periodicity, bool> ReadVariableRegistrationElements(byte[] data, ref int offset)
        {
            string variableName = data.ReadString(ref offset);
            Periodicity periodicity = data.ReadPeriodicity(ref offset);
            bool isRegister = data.ReadBoolean(ref offset);

            return new Tuple<string, Periodicity, bool>(variableName, periodicity, isRegister);
        }
    }
}
