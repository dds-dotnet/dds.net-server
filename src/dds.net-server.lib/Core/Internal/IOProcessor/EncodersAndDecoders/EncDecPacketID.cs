using DDS.Net.Server.Core.Internal.IOProcessor.Types;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecPacketID
    {
        //- 
        //- PacketID
        //- 
        public static PacketID ReadPacketID(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 2);

            int v = data[offset++];
            v = (v << 8) | data[offset++];

            if (v >= 0 && v < (int)PacketID.UNKNOWN)
            {
                return (PacketID)v;
            }

            return PacketID.UNKNOWN;
        }
        public static void WritePacketID(this byte[] data, ref int offset, PacketID value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 1);

            int v = (int)value;

            data[offset++] = (byte)((v >> 8) & 0x0ff);
            data[offset++] = (byte)((v >> 0) & 0x0ff);
        }
    }
}
