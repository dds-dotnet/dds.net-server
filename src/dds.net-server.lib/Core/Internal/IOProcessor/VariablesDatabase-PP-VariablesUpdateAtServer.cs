using DDS.Net.Server.Core.Internal.Base;
using DDS.Net.Server.Core.Internal.Base.Entities;
using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Core.Internal.IOProcessor.Types;
using DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable;
using DDS.Net.Server.Entities;

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
             *     [UnsignedWord: Variable Id] - Optional
             *     [String: Error Message]
             *     ...
             *     ...
             *     
             *     Server -> Client : PacketId.ErrorResponseFromServer
             *     ---------------------------------------------------
             *     [String: Error Message]
             *     
             */

            Dictionary<ushort, string> errorMessages = new();

            try
            {
                while (offset < data.Length)
                {
                    (ushort variableId, VariableType variableType) =
                        ReadVariableValueInformationElements(data, ref offset);

                    BaseVariable variable = GetVariableWithId(variableId);

                    if (variable.VariableType == variableType)
                    {

                    }
                    else
                    {
                        if (errorMessages.ContainsKey(variableId) == false)
                        {
                            errorMessages.Add(
                                variableId,
                                $"Variable type should be {variable.VariableType}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SendErrorPacket(clientRef, ex.Message);
                return;
            }
        }
        /// <summary>
        /// Reads variable value information elements from the given data buffer.
        /// </summary>
        /// <param name="data">Data buffer.</param>
        /// <param name="offset">Reading offset in the buffer.</param>
        /// <returns>(
        /// ushort: variableId,
        /// VariableType: variableType
        /// )</returns>
        private static (ushort variableId, VariableType variableType)
            ReadVariableValueInformationElements(byte[] data, ref int offset)
        {
            ushort variableId = data.ReadUnsignedWord(ref offset);
            VariableType variableType = data.ReadVariableType(ref offset);

            return (variableId, variableType);
        }
    }
}
