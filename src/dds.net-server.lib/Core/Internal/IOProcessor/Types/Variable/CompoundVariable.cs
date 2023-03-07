namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class CompoundVariable : BaseVariable
    {
        public CompoundVariable(ushort id, string name) : base(id, name)
        {

        }

        protected override int GetTypeSizeOnBuffer()
        {
            throw new NotImplementedException();
        }

        protected override int GetValueSizeOnBuffer()
        {
            throw new NotImplementedException();
        }

        protected override void WriteTypeOnBuffer(ref byte[] buffer, ref int offset)
        {
            throw new NotImplementedException();
        }

        protected override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
            throw new NotImplementedException();
        }
    }
}
