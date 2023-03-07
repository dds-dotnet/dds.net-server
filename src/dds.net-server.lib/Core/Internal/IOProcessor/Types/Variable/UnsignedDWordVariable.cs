using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class UnsignedDWordVariable : BasePrimitive
    {
        public uint Value { get; set; }

        public UnsignedDWordVariable(ushort id, string name) : base(id, name)
        {
            PrimitiveType = PrimitiveType.UnsignedDWord;
        }

        protected override int GetValueSizeOnBuffer()
        {
            return 4;
        }

        protected override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteUnsignedDWord(ref offset, Value);
        }
    }
}
