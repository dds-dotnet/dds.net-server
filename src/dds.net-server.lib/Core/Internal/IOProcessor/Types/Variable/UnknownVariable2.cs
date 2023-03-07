using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class UnknownVariable2 : BasePrimitive
    {
        public UnknownVariable2(ushort id, string name) : base(id, name)
        {
            PrimitiveType = PrimitiveType.UnknownPrimitiveType;
        }

        protected override int GetValueSizeOnBuffer()
        {
            throw new Exception("Unknown variable - size cannot be estimated");
        }

        protected override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
            throw new Exception("Unknown variable - cannot be written on buffer");
        }
    }
}
