using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal abstract class BaseVariable
    {
        /// <summary>
        /// Identifier for the variable
        /// </summary>
        public ushort Id { get; private set; }

        /// <summary>
        /// Name associated with the variable
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Main type of the variable i.e., primitive or compound, etc.
        /// </summary>
        public VariableType VariableType { get; protected set; }


        /// <summary>
        /// Number of bytes ID takes on buffer
        /// </summary>
        private static readonly int IdSizeOnBuffer = sizeof(short);
        /// <summary>
        /// Number of bytes VariableType takes on buffer
        /// </summary>
        private static readonly int VariableTypeSizeOnBuffer = VariableType.Primitive.GetSizeOnBuffer();


        /// <summary>
        /// List of value providers for the variable
        /// </summary>
        public List<VariableProvider> Providers { get; set; } = new();

        /// <summary>
        /// Last update timestamp
        /// </summary>
        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Timestamp for when the variable was sent to subscribers
        /// </summary>
        public DateTime LastSentToSubscribers { get; set; } = DateTime.Now;


        public BaseVariable(ushort id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Total size => [ID]-[Variable Type]-[Type]-[Value]
        /// </summary>
        /// <returns></returns>
        public int GetSizeOnBuffer()
        {
            return
                IdSizeOnBuffer +
                VariableTypeSizeOnBuffer +
                GetTypeSizeOnBuffer() +
                GetValueSizeOnBuffer();
        }
        /// <summary>
        /// Write everything including ID, Type and Value on the buffer
        /// </summary>
        /// <param name="buffer">Buffer on which to write</param>
        /// <param name="offset">Offset in the buffer - also updated after writing</param>
        public void WriteOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteUnsignedWord(ref offset, Id);
            buffer.WriteVariableType(ref offset, VariableType);

            WriteTypeOnBuffer(ref buffer, ref offset);
            WriteValueOnBuffer(ref buffer, ref offset);
        }

        /// <summary>
        /// Required size of type on the buffer
        /// </summary>
        /// <returns>Size in bytes required to write type on buffer</returns>
        protected abstract int GetTypeSizeOnBuffer();
        /// <summary>
        /// Required size of value on the buffer
        /// </summary>
        /// <returns>Number of bytes required to write value on the buffer</returns>
        protected abstract int GetValueSizeOnBuffer();
        /// <summary>
        /// Write type of data on the buffer
        /// </summary>
        /// <param name="buffer">Buffer on which to write</param>
        /// <param name="offset">Offset in the buffer - also updated after writing</param>
        protected abstract void WriteTypeOnBuffer(ref byte[] buffer, ref int offset);
        /// <summary>
        /// Write value onto the buffer
        /// </summary>
        /// <param name="buffer">Buffer on which to write</param>
        /// <param name="offset">Offset in the buffer - also updated after writing</param>
        protected abstract void WriteValueOnBuffer(ref byte[] buffer, ref int offset);
    }
}
