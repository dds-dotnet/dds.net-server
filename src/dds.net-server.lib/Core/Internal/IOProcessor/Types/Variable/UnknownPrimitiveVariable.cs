using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class UnknownPrimitiveVariable : BasePrimitive
    {
        public UnknownPrimitiveVariable(ushort id, string name) : base(id, name)
        {
            PrimitiveType = PrimitiveType.UnknownPrimitiveType;
        }

        public override int GetValueSizeOnBuffer()
        {
            throw new Exception("Unknown variable - size cannot be estimated");
        }

        public override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
            throw new Exception("Unknown variable - cannot be written on buffer");
        }
    }
}
