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
            List<BaseVariable> updatedVariables = new();

            //- 
            //- Processing the values
            //- 

            try
            {
                while (offset < data.Length)
                {
                    (ushort variableId, VariableType variableType) =
                        ReadVariableValueInformationElements(data, ref offset);

                    BaseVariable variable = GetVariableWithId(variableId);

                    if (variable.VariableType == variableType)
                    {
                        //- 
                        //- Variable has already specified type
                        //- 

                        if (variableType == VariableType.Primitive)
                        {

                        }
                        else if (variableType == VariableType.Compound)
                        {

                        }
                        else
                        {
                            throw new Exception($"Variable type {variableType} cannot be processed");
                        }
                    }
                    else if (
                        variable.VariableType == VariableType.UnknownVariableType &&
                        variableType != VariableType.UnknownVariableType)
                    {
                        //- 
                        //- Variable type was previously unknown but it is known now
                        //- 
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
                SendErrorMessage(clientRef, ex.Message);

                if (updatedVariables.Count > 0)
                {
                    SendUpdatedVariables(updatedVariables);
                    updatedVariables.Clear();
                }
            }

            //- 
            //- Sending back the error response
            //- 

            SendVariablesUpdateErrorMessages(clientRef, errorMessages);

            //- 
            //- Sending the updated variables - if available
            //- 

            if (updatedVariables.Count > 0)
            {
                SendUpdatedVariables(updatedVariables);
            }
        }

        /// <summary>
        /// Sends error messages when there is an error while updating variable values.
        /// </summary>
        /// <param name="clientRef">Client's address.</param>
        /// <param name="errorMessages">Error messages (variable ID => error message)</param>
        private void SendVariablesUpdateErrorMessages(string clientRef, Dictionary<ushort, string> errorMessages)
        {
            //- Calculating required size for buffer

            int sizeRequired = 0;

            foreach (KeyValuePair<ushort, string> errorInfo in errorMessages)
            {
                sizeRequired +=
                    2 +                         // Id size on buffer
                    2 + errorInfo.Value.Length; // string size on buffer
            }

            //- Sending response

            if (sizeRequired > 0)
            {
                byte[] responseBuffer = new byte[sizeRequired];
                int responseBufferOffset = 0;

                //- Filling the response buffer

                foreach (KeyValuePair<ushort, string> varInfo in errorMessages)
                {
                    responseBuffer.WriteUnsignedWord(ref responseBufferOffset, varInfo.Key);
                    responseBuffer.WriteString(ref responseBufferOffset, varInfo.Value);
                }

                OutputQueue.Enqueue(new DataToClient(clientRef, responseBuffer));
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
