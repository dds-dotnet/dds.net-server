using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class QWordVariable : BasePrimitive
    {
        public long Value { get; set; }

        public QWordVariable(ushort id, string name) : base(id, name)
        {
            PrimitiveType = PrimitiveType.QWord;
        }

        public override int GetValueSizeOnBuffer()
        {
            return 8;
        }

        public override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteQWord(ref offset, Value);
        }
    }
}
