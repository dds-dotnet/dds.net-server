using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types
{
    internal class Variable
    {
        public VariableType VariableType { get; set; } = VariableType.UNKNOWN;

        public ushort Id { get; set; }

        public long SignedValue { get; set; }
        public ulong UnsignedValue { get; set; }
        public double FloatingPointValue { get; set; }
        
        public List<VariableProvider> Providers { get; set; } = new();
        public List<VariableSubscriber> Subscribers { get; set; } = new();
    }
}
