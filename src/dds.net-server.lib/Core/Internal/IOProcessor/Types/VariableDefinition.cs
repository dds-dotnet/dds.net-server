namespace DDS.Net.Server.Core.Internal.IOProcessor.Types
{
    internal class VariableDefinition
    {
        public VariableType VariableType { get; set; } = VariableType.UNKNOWN;

        public ushort ID { get; set; }
        public string Name { get; set; } = "";
        
        public List<VariableProvider> Providers { get; set; } = new();
        public List<VariableSubscriber> Subscribers { get; set; } = new();
    }
}
