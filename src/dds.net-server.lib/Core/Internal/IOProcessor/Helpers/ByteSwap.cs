namespace DDS.Net.Server.Core.Internal.IOProcessor.Helpers
{
    internal static class ByteSwap
    {
        public static byte[] SwapBytes(this byte[] data)
        {
            for (int i = 0; i < data.Length / 2; i++)
            {
                byte temp = data[i];
                data[i] = data[data.Length - i - 1];
                data[data.Length - i - 1] = temp;
            }

            return data;
        }
    }
}
