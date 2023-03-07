using DDS.Net.Server.Core.Internal.IOProcessor.Types;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders
{
    internal static class EncDecPacketId
    {
        //- 
        //- PacketId
        //- 
        public static PacketId ReadPacketId(this byte[] data, ref int offset)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 2);

            int v = data[offset++];
            v = (v << 8) | data[offset++];

            if (v >= 0 && v < (int)PacketId.UNKNOWN)
            {
                return (PacketId)v;
            }

            return PacketId.UNKNOWN;
        }
        public static void WritePacketId(this byte[] data, ref int offset, PacketId value)
        {
            data.ThrowIfNotHavingRequiredBytes(ref offset, 2);

            int v = (int)value;

            data[offset++] = (byte)((v >> 8) & 0x0ff);
            data[offset++] = (byte)((v >> 0) & 0x0ff);
        }
        public static int GetSizeOnBuffer(this PacketId packetId)
        {
            return 2;
        }
    }
}
