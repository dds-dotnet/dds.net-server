using System.Diagnostics;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecMessageHeader
    {
        //- 
        //- MessageHeader
        //- 

        /// <summary>
        /// Reads total number of bytes in the message from the data buffer
        /// and updates the offset past the header.
        /// </summary>
        /// <param name="data">The buffer containing the data.</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer.</param>
        /// <returns>Total bytes in the message if started with specified prefix.</returns>
        internal static int ReadTotalBytesInMessage(this byte[] data, ref int offset)
        {
            Debug.Assert(data != null);
            Debug.Assert(offset + 6 <= data.Length);

            if (data[offset] == '#' && data[offset + 1] == '#')
            {
                offset += 2;

                int v = data[offset++];
                v = (v << 8) | data[offset++];
                v = (v << 8) | data[offset++];
                v = (v << 8) | data[offset++];

                return v;
            }

            throw new Exception($"The message is not starting from given offset {offset}");
        }

        /// <summary>
        /// Writes message starting indicator and total number of bytes
        /// in the message to the given data buffer.
        /// </summary>
        /// <param name="data">The buffer containing the data.</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer.</param>
        /// <param name="totalBytes">Total bytes that the message needs to contain.</param>
        internal static void WriteMessageHeader(this byte[] data, ref int offset, int totalBytes)
        {
            Debug.Assert(data != null);
            Debug.Assert(offset + 6 <= data.Length);

            data[offset++] = (byte)'#';
            data[offset++] = (byte)'#';

            data[offset++] = (byte)((totalBytes >> 24) & 0x0ff);
            data[offset++] = (byte)((totalBytes >> 16) & 0x0ff);
            data[offset++] = (byte)((totalBytes >> 8) & 0x0ff);
            data[offset++] = (byte)((totalBytes >> 0) & 0x0ff);
        }

        /// <summary>
        /// Total size in bytes that the Message Header requires on a buffer.
        /// </summary>
        /// <returns>Total number of bytes required on a buffer.</returns>
        internal static int GetMessageHeaderSizeOnBuffer()
        {
            return 2 + // The starting '##'
                   4;  // Total bytes in message
        }
    }
}
