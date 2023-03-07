using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class UnsignedWordVariable : BasePrimitive
    {
        public ushort Value { get; set; }

        public UnsignedWordVariable(ushort id, string name) : base(id, name)
        {
            PrimitiveType = PrimitiveType.UnsignedWord;
        }

        public override int GetSizeOnBuffer()
        {
            return 2 + PrimitiveType.GetSizeOnBuffer() + IdSizeOnBuffer;
        }

        public override void WriteOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteUnsignedWord(ref offset, Value);
        }
    }
}
