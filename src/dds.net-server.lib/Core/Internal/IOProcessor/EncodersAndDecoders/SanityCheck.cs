namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class SanityCheck
    {
        public static byte[] CheckForRequiredSize(this byte[] data, ref int offset, int requiredSize)
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
