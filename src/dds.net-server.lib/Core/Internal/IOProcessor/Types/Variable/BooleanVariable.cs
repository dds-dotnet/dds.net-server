using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class BooleanVariable : BaseVariable
    {
        public PrimitiveType PrimitiveType { get; private set; } = PrimitiveType.Boolean;

        public bool Value { get; set; }

        public BooleanVariable(ushort id, string name) : base(id, name)
        {
        }

        public override int GetSizeOnBuffer()
        {
            return 1;
        }

        public override void WriteOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteBoolean(ref offset, Value);
        }
    }
}
