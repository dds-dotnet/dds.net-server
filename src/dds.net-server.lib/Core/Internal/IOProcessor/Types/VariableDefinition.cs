namespace DDS.Net.Server.Core.Internal.IOProcessor.Types
{
    internal class VariableDefinition
    {
        public VariableType VariableType { get; set; } = VariableType.UNKNOWN;
        public List<VariableProvider> Providers { get; set; } = new();
        public List<VariableSubscriber> Subscribers { get; set; } = new();
    }
}
