using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class BooleanVariable : BasePrimitive
    {
        public bool Value { get; set; }

        public BooleanVariable(ushort id, string name) : base(id, name)
        {
            PrimitiveType = PrimitiveType.Boolean;
        }

        public override int GetValueSizeOnBuffer()
        {
            return 1;
        }

        public override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteBoolean(ref offset, Value);
        }
    }
}
