using DDS.Net.Server.Core.Internal.IOProcessor.Types;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecPeriodicity
    {
        //- 
        //- Periodicity
        //- 

        /// <summary>
        /// Reads Periodicity from the data buffer and updates the offset past the Periodicity
        /// </summary>
        /// <param name="data">The buffer containing data</param>
        /// <param name="offset">offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <returns>Periodicity</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static Periodicity ReadPeriodicity(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            int v = data[offset++];

            if (v >= 0 && v <= (int)Periodicity.Lowest)
            {
                return (Periodicity)v;
            }

            return Periodicity.OnChange;
        }
        /// <summary>
        /// Writes Periodicity to the given data buffer
        /// </summary>
        /// <param name="data">The buffer containing data</param>
        /// <param name="offset">Offset in the data buffer - updated afterwards to point
        /// to the next element in the buffer</param>
        /// <param name="value">value to be written to the buffer</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void WritePeriodicity(this byte[] data, ref int offset, Periodicity value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            data[offset++] = (byte)value;
        }
        /// <summary>
        /// Size in bytes Periodicity requires on a buffer
        /// </summary>
        /// <param name="_"></param>
        /// <returns>Number of bytes required on the buffer</returns>
        public static int GetSizeOnBuffer(this Periodicity _)
        {
            return 1;
        }
    }
}
