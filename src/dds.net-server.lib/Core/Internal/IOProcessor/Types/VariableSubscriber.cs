namespace DDS.Net.Server.Core.Internal.IOProcessor.Types
{
    internal class VariableSubscriber
    {
        public ushort VariableId { get; private set; }
        public string ClientRef { get; private set; }
        public Periodicity Periodicity { get; private set; }

        public VariableSubscriber(
            ushort variableId,
            string clientRef,
            Periodicity periodicity)
        {
            VariableId = variableId;
            ClientRef = clientRef;
            Periodicity = periodicity;
        }
    }
}
