using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class UnknownVariable : BaseVariable
    {
        public UnknownVariable(ushort id, string name) : base(id, name)
        {
            VariableType = VariableType.UnknownVariableType;
        }

        public override int GetTypeSizeOnBuffer()
        {
            return 0;
        }

        public override int GetValueSizeOnBuffer()
        {
            return 0;
        }

        public override void WriteTypeOnBuffer(ref byte[] buffer, ref int offset)
        {
        }

        public override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
        }
    }
}
