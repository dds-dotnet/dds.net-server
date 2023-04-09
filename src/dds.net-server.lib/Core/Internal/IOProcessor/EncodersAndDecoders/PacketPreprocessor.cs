using DDS.Net.Server.Core.Internal.Base.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal class PacketPreprocessor
    {
        private Mutex mutex = new();
        private Dictionary<string, byte[]> previousData = new();
        private Dictionary<string, int> previousDataStartIndex = new();
        private Dictionary<string, int> previousNextWriteIndex = new();

        internal void AddData(DataFromClient data)
        {
            lock (mutex)
            {
                //- 
                //- Buffer: [ - - - - - - - - - - - - - - - - - - - - - - - - - - - ]
                //-                   Data: - - - - - -
                //-           Start Index _/            \_ Next Write Index
                //- 

                byte[] buffer = null!;
                int bufferStartIndex = 0;
                int bufferNextWriteIndex = 0;

                //- 
                //- No previous data available
                //- 
                if (previousData.ContainsKey(data.ClientRef) == false)
                {
                    buffer = new byte[data.Data.Length];
                    bufferStartIndex = 0;
                    bufferNextWriteIndex = 0;

                    previousData.Add(data.ClientRef, buffer);
                    previousDataStartIndex.Add(data.ClientRef, bufferStartIndex);
                    previousNextWriteIndex.Add(data.ClientRef, bufferNextWriteIndex);
                }
                //- 
                //- We have some previous data
                //- 
                else
                {
                    buffer = previousData[data.ClientRef];
                    bufferStartIndex = previousDataStartIndex[data.ClientRef];
                    bufferNextWriteIndex = previousNextWriteIndex[data.ClientRef];
                }

                //- 
                //- Compacting the buffer
                //- 
                if (bufferStartIndex > 0)
                {
                    for (int i = 0; i < (bufferNextWriteIndex - bufferStartIndex); i++)
                    {
                        buffer[i] = buffer[bufferStartIndex + i];
                    }

                    bufferNextWriteIndex -= bufferStartIndex;
                    bufferStartIndex = 0;
                }
                else
                {
                    bufferNextWriteIndex = 0;
                    bufferStartIndex = 0;
                }

                //- 
                //- Do we have enough space for the data?
                //- 
                if ((buffer.Length - bufferNextWriteIndex) >= data.Data.Length)
                {
                    for (int i = 0; i < data.Data.Length; i++)
                    {
                        buffer[bufferNextWriteIndex++] = data.Data[i];
                    }

                    previousDataStartIndex[data.ClientRef] = bufferStartIndex;
                    previousNextWriteIndex[data.ClientRef] = bufferNextWriteIndex;
                }
                //- 
                //- No, we do not have enough space for data.
                //- 
                else
                {
                    byte[] newBuffer = new byte[buffer.Length + data.Data.Length];
                    int newBufferStartIndex = 0;
                    int newBufferNextWriteIndex = 0;

                    // Copy old data
                    for (int i = 0; i < bufferNextWriteIndex; i++)
                    {
                        newBuffer[newBufferNextWriteIndex++] = buffer[i];
                    }

                    // Copy new data
                    for (int i = 0; i < data.Data.Length; i++)
                    {
                        newBuffer[newBufferNextWriteIndex++] = data.Data[i];
                    }

                    previousData[data.ClientRef] = newBuffer;
                    previousDataStartIndex[data.ClientRef] = newBufferStartIndex;
                    previousNextWriteIndex[data.ClientRef] = newBufferNextWriteIndex;
                }
            }
        }

        internal byte[] GetSingleMessage(string clientRef)
        {
            lock (mutex)
            {
                //- 
                //- Do we have any data available?
                //- 
                if (previousData.ContainsKey(clientRef))
                {
                    byte[] buffer = previousData[clientRef];
                    int bufferStartIndex = previousDataStartIndex[clientRef];
                    int bufferNextWriteIndex = previousNextWriteIndex[clientRef];

                    //- 
                    //- Do we have full header?
                    //- 
                    
                    int index = bufferStartIndex;

                    while (index < (bufferNextWriteIndex - 1))
                    {
                        // Finding '##'

                        if (buffer[index] == '#' &&
                            buffer[index + 1] == '#')
                        {
                            bufferStartIndex = index;
                            int readableBytes = bufferNextWriteIndex - index;

                            if (readableBytes >= EncDecMessageHeader.GetMessageHeaderSizeOnBuffer())
                            {
                                int dataBytes = buffer.ReadTotalBytesInMessage(ref index);
                                int availableBytes = bufferNextWriteIndex - index;

                                if (availableBytes >= dataBytes)
                                {
                                    byte[] packet = new byte[dataBytes];

                                    for (int i = 0; i < dataBytes; i++)
                                    {
                                        packet[i] = buffer[index++];
                                    }

                                    previousDataStartIndex[clientRef] = index;

                                    return packet;
                                }
                            }

                            break;
                        }

                        index++;
                    }

                    return null!;
                }
                //- 
                //- No, we do not have any data available.
                //- 
                else
                {
                    return null!;
                }
            }
        }
    }
}
