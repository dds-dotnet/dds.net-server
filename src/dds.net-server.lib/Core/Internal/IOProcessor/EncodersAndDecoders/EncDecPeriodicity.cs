using DDS.Net.Server.Core.Internal.IOProcessor.Types;
using System.Diagnostics;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecPeriodicity
    {
        //- 
        //- Periodicity
        //- 

        /// <summary>
        /// Reads <c cref="Periodicity">Periodicity</c> from the data buffer
        /// and updates the offset past the <c cref="Periodicity">Periodicity</c>.
        /// </summary>
        /// <param name="data">The buffer containing data.</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer.</param>
        /// <returns><c cref="Periodicity">Periodicity</c></returns>
        internal static Periodicity ReadPeriodicity(this byte[] data, ref int offset)
        {
            Debug.Assert(data != null);
            Debug.Assert(offset + 1 <= data.Length);

            int v = data[offset++];

            if (v >= 0 && v <= (int)Periodicity.Lowest)
            {
                return (Periodicity)v;
            }

            return Periodicity.OnChange;
        }

        /// <summary>
        /// Writes <c cref="Periodicity">Periodicity</c> to the given data buffer.
        /// </summary>
        /// <param name="data">The buffer containing data.</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer.</param>
        /// <param name="value">Value to be written to the buffer.</param>
        internal static void WritePeriodicity(this byte[] data, ref int offset, Periodicity value)
        {
            Debug.Assert(data != null);
            Debug.Assert(offset + 1 <= data.Length);

            data[offset++] = (byte)value;
        }

        /// <summary>
        /// Size in bytes that <c cref="Periodicity">Periodicity</c> requires on a buffer.
        /// </summary>
        /// <param name="_"></param>
        /// <returns>Number of bytes required on the buffer</returns>
        internal static int GetSizeOnBuffer(this Periodicity _)
        {
            return 1;
        }
    }
}
