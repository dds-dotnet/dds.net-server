using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal abstract class BasePrimitive : BaseVariable
    {
        public PrimitiveType PrimitiveType { get; protected set; } = PrimitiveType.UNKNOWN;

        public BasePrimitive(ushort id, string name) : base(id, name)
        {
        }

        protected override int GetTypeSizeOnBuffer()
        {
            return PrimitiveType.GetSizeOnBuffer();
        }

        public override void WriteTypeOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteVariableType(ref offset, PrimitiveType);
        }
    }
}
