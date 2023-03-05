using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types
{
    internal class PrimitiveStringVariable
    {
        public PrimitiveType PrimitiveType { get; private set; } = PrimitiveType.String;

        public string Value { get; set; }

        public PrimitiveStringVariable()
        {
            Value = string.Empty;
        }
    }
}
