using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class UNKNOWNVariable : BasePrimitive
    {
        public UNKNOWNVariable(ushort id, string name) : base(id, name)
        {
            PrimitiveType = PrimitiveType.UNKNOWN;
        }

        public override int GetSizeOnBuffer()
        {
            throw new Exception("Unknown variable - size cannot be estimated");
        }

        public override void WriteOnBuffer(ref byte[] buffer, ref int offset)
        {
            throw new Exception("Unknown variable - cannot be written on buffer");
        }
    }
}
