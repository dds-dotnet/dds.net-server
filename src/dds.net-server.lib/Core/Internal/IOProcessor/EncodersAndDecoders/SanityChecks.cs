namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class SanityChecks
    {
        /// <summary>
        /// Checks if the data buffer can be read / written with required number of bytes
        /// </summary>
        /// <param name="data">The data buffer</param>
        /// <param name="offset">Offset in the buffer</param>
        /// <param name="requiredSize">Number of bytes required in the buffer</param>
        /// <returns>The same data buffer - for chaining the call with others</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static byte[] ThrowIfNotHavingRequiredBytes(this byte[] data, ref int offset, int requiredSize)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(offset),
                    $"Offset is negative: {offset}");
            }

            if (offset + requiredSize - 1 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(offset),
                    $"Array of {data.Length} bytes requires to have data of {requiredSize} bytes starting at {offset} byte offset");
            }

            return data;
        }
    }
}
