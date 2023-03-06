using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class DoubleVariable<T> : BaseVariable
    {
        public PrimitiveType PrimitiveType { get; private set; } = PrimitiveType.Double;

        public double Value { get; set; }

        public DoubleVariable(ushort id, string name) : base(id, name)
        {
        }

        public override int GetSizeOnBuffer()
        {
            return 8;
        }

        public override void WriteOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteDouble(ref offset, Value);
        }
    }
}
