namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class SanityCheck
    {
        public static void CheckRequiredSize(this byte[] data, ref int offset, int requiredSize)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset + requiredSize - 1 >= data.Length)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(offset),
                    $"Array size is {data.Length}, needed data size is {requiredSize} at offset {offset}");
            }
        }
    }
}
