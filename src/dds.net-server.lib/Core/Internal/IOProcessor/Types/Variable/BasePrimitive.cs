using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal abstract class BasePrimitive : BaseVariable
    {
        public PrimitiveType PrimitiveType { get; protected set; } = PrimitiveType.UNKNOWN;

        public BasePrimitive(ushort id, string name) : base(id, name)
        {
        }
    }
}
