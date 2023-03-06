using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class StringVariable : BaseVariable
    {
        public PrimitiveType PrimitiveType { get; private set; } = PrimitiveType.String;

        public string Value { get; set; }

        public StringVariable(ushort id, string name) : base(id, name)
        {
            Value = string.Empty;
        }
    }
}
