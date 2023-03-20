using DDS.Net.Server.Core.Internal.Base.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class PacketPreprocessor
    {
        private static Mutex mutex = new();
        private static Dictionary<string, byte[]> previousData = new();
        private static Dictionary<string, int> previousDataStartIndex = new();
        private static Dictionary<string, int> previousNextWriteIndex = new();

        internal static void AddData(DataFromClient data)
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
                int timesShifted = 0;
                for (int i = 0; i < bufferStartIndex; i++)
                {
                    buffer[i] = buffer[bufferStartIndex + i];
                    timesShifted++;
                }

                bufferStartIndex = 0;
                bufferNextWriteIndex -= timesShifted;

                //- 
                //- Do we have enough space for the data?
                //- 
                if ((buffer.Length - bufferNextWriteIndex) >= data.Data.Length)
                {
                    for (int i = 0; i < data.Data.Length; i++)
                    {
                        buffer[bufferNextWriteIndex++] = data.Data[i];
                    }
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

        internal static byte[] GetSingleMessage(string clientRef)
        {
            lock (mutex)
            {
                return null!;
            }
        }
    }
}
