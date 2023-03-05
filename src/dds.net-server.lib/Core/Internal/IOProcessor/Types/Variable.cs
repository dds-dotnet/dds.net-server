namespace DDS.Net.Server.Core.Internal.IOProcessor.Types
{
    internal abstract class Variable
    {
        public ushort Id { get; private set; }
        public string Name { get; private set; }

        public List<VariableProvider> Providers { get; set; } = new();
        public List<VariableSubscriber> Subscribers { get; set; } = new();

        public Variable(ushort id, string name)
        {
            Id = id;
            Name = name;

        }
    }
}
