using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class WordVariable : BasePrimitive
    {
        public short Value { get; set; }

        public WordVariable(ushort id, string name) : base(id, name)
        {
            PrimitiveType = PrimitiveType.Word;
        }

        protected override int GetValueSizeOnBuffer()
        {
            return 2;
        }

        protected override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteWord(ref offset, Value);
        }
    }
}
