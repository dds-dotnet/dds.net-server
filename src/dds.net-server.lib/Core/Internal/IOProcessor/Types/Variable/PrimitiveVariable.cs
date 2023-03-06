using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class PrimitiveVariable<T> : BaseVariable
        where T : struct
    {
        public PrimitiveType PrimitiveType { get; private set; }

        public T Value { get; set; }

        public PrimitiveVariable(ushort id, string name) : base(id, name)
        {
            if (typeof(T) == typeof(bool)) { PrimitiveType = PrimitiveType.Boolean; }
            else if (typeof(T) == typeof(sbyte)) { PrimitiveType = PrimitiveType.Byte; }
            else if (typeof(T) == typeof(short)) { PrimitiveType = PrimitiveType.Word; }
            else if (typeof(T) == typeof(int)) { PrimitiveType = PrimitiveType.DWord; }
            else if (typeof(T) == typeof(long)) { PrimitiveType = PrimitiveType.QWord; }
            else if (typeof(T) == typeof(byte)) { PrimitiveType = PrimitiveType.UnsignedByte; }
            else if (typeof(T) == typeof(ushort)) { PrimitiveType = PrimitiveType.UnsignedWord; }
            else if (typeof(T) == typeof(uint)) { PrimitiveType = PrimitiveType.UnsignedDWord; }
            else if (typeof(T) == typeof(ulong)) { PrimitiveType = PrimitiveType.UnsignedQWord; }
            else if (typeof(T) == typeof(float)) { PrimitiveType = PrimitiveType.Single; }
            else if (typeof(T) == typeof(double)) { PrimitiveType = PrimitiveType.Double; }
            else { PrimitiveType = PrimitiveType.UNKNOWN; }
        }
    }
}
