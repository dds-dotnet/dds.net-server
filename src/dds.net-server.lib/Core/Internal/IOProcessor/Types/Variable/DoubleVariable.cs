using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class DoubleVariable : BasePrimitive
    {
        public double Value { get; set; }

        public DoubleVariable(ushort id, string name) : base(id, name)
        {
            PrimitiveType = PrimitiveType.Double;
        }

        protected override int GetValueSizeOnBuffer()
        {
            return 8;
        }

        protected override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteDouble(ref offset, Value);
        }
    }
}
