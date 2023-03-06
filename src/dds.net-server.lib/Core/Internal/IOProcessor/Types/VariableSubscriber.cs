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

        public static bool operator ==(VariableSubscriber obj1, VariableSubscriber obj2)
        {
            if (obj1 != null! && obj2 != null!)
            {
                return
                    obj1.VariableId  == obj2.VariableId &&
                    obj1.ClientRef   == obj2.ClientRef &&
                    obj1.Periodicity == obj2.Periodicity;
            }

            return false;
        }

        public static bool operator !=(VariableSubscriber obj1, VariableSubscriber obj2)
        {
            if (obj1 != null! && obj2 != null!)
            {
                bool equals =
                    obj1.VariableId  == obj2.VariableId &&
                    obj1.ClientRef   == obj2.ClientRef &&
                    obj1.Periodicity == obj2.Periodicity;

                return !equals;
            }

            return true;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (obj is VariableSubscriber other)
            {
                return this == other;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return
                VariableId.GetHashCode() ^
                ClientRef.GetHashCode() ^
                Periodicity.GetHashCode();
        }
    }
}
