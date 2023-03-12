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
            return 0;
        }

        public override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
        }
    }
}
