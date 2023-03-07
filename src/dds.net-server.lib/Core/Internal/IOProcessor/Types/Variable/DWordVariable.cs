using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class DWordVariable : BasePrimitive
    {
        public int Value { get; set; }

        public DWordVariable(ushort id, string name) : base(id, name)
        {
            PrimitiveType = PrimitiveType.DWord;
        }

        protected override int GetValueSizeOnBuffer()
        {
            return 4;
        }

        protected override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteDWord(ref offset, Value);
        }
    }
}
