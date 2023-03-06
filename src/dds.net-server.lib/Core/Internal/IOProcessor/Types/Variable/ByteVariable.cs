using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class ByteVariable<T> : BaseVariable
    {
        public PrimitiveType PrimitiveType { get; private set; } = PrimitiveType.Byte;

        public sbyte Value { get; set; }

        public ByteVariable(ushort id, string name) : base(id, name)
        {
        }

        public override int GetSizeOnBuffer()
        {
            return 1;
        }

        public override void WriteOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteByte(ref offset, Value);
        }
    }
}
