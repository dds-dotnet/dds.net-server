using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class SingleVariable : BasePrimitive
    {
        public float Value { get; set; }

        public SingleVariable(ushort id, string name) : base(id, name)
        {
            PrimitiveType = PrimitiveType.Single;
        }

        protected override int GetValueSizeOnBuffer()
        {
            return 4;
        }

        protected override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteSingle(ref offset, Value);
        }
    }
}
