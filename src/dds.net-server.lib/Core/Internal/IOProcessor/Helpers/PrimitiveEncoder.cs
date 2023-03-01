namespace DDS.Net.Server.Core.Internal.IOProcessor.Helpers
{
    internal static class PrimitiveEncoder
    {
        public static sbyte ReadByte(this byte[] data, ref int offset)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            return (sbyte)data[offset++];
        }
    }
}
