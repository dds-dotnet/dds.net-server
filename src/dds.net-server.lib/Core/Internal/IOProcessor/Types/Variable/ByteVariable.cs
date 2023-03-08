using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class ByteVariable : BasePrimitive
    {
        public sbyte Value { get; set; }

        public ByteVariable(ushort id, string name) : base(id, name)
        {
            PrimitiveType = PrimitiveType.Byte;
        }

        public override int GetValueSizeOnBuffer()
        {
            return 1;
        }

        public override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteByte(ref offset, Value);
        }
    }
}
