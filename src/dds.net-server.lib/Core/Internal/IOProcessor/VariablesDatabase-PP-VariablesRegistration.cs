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
             *     [Boolean: Provider or Consumer]   - True = Client is provider, False = Client is consumer
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
                    (string variableName,
                     Periodicity periodicity,
                     bool isClientProvider,
                     bool isRegister) = ReadVariableRegistrationElements(data, ref offset);

                    if (isRegister)
                    {
                        ushort varId = RegisterVariableClient(clientRef, variableName, periodicity, isClientProvider);

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
                SendErrorMessage(clientRef, ex.Message);
                return;
            }

            SendVariablesRegistrationResponse(clientRef, registeredVariables, unregisteredVariables);
        }

        /// <summary>
        /// Reads variable registration elements from the given packet.
        /// </summary>
        /// <param name="data">Data buffer.</param>
        /// <param name="offset">Reading offset in the buffer.</param>
        /// <returns>(
        /// string: variableName,
        /// Periodicity: update periodicity,
        /// bool: is client a provider
        /// bool: is registering
        /// )</returns>
        private static (string variableName, Periodicity periodicity, bool isClientProvider, bool isRegister)
            ReadVariableRegistrationElements(byte[] data, ref int offset)
        {
            string variableName = data.ReadString(ref offset);
            Periodicity periodicity = data.ReadPeriodicity(ref offset);
            bool isClientProvider = data.ReadBoolean(ref offset);
            bool isRegister = data.ReadBoolean(ref offset);

            return (variableName, periodicity, isClientProvider, isRegister);
        }
    }
}
