using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class UnsignedByteVariable : BasePrimitive
    {
        public byte Value { get; set; }

        public UnsignedByteVariable(ushort id, string name) : base(id, name)
        {
            PrimitiveType = PrimitiveType.UnsignedByte;
        }

        protected override int GetValueSizeOnBuffer()
        {
            return 1;
        }

        protected override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteUnsignedByte(ref offset, Value);
        }
    }
}
