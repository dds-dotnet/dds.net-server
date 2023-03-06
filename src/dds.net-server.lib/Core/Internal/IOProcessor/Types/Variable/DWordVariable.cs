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

        public override int GetSizeOnBuffer()
        {
            return 4;
        }

        public override void WriteOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteDWord(ref offset, Value);
        }
    }
}
