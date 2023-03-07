using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal abstract class BaseVariable
    {
        public ushort Id { get; private set; }
        public string Name { get; private set; }

        public VariableType VariableType { get; protected set; }

        private static int IdSizeOnBuffer = sizeof(short);

        public List<VariableProvider> Providers { get; set; } = new();

        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;

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
                GetVariableTypeSizeOnBuffer() +
                GetTypeSizeOnBuffer() +
                GetValueSizeOnBuffer();
        }
        /// <summary>
        /// Required size of variable type on the buffer
        /// </summary>
        /// <returns>Size in bytes required to write variable type on buffer</returns>
        private int GetVariableTypeSizeOnBuffer()
        {
            return VariableType.GetSizeOnBuffer();
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
        /// Write everything including ID, Type and Value on the buffer
        /// </summary>
        /// <param name="buffer">Buffer on which to write</param>
        /// <param name="offset">Offset in the buffer - also updated after writing</param>
        public void WriteOnBuffer(ref byte[] buffer, ref int offset)
        {
            WriteIdOnBuffer(ref buffer, ref offset);
            WriteVariableTypeOnBuffer(ref buffer, ref offset);
            WriteTypeOnBuffer(ref buffer, ref offset);
            WriteValueOnBuffer(ref buffer, ref offset);
        }
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
        /// <summary>
        /// Writing ID on the buffer
        /// </summary>
        /// <param name="buffer">Buffer on which value is to be written</param>
        /// <param name="offset">offset on which the value is to be written in buffer</param>
        private void WriteIdOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteUnsignedWord(ref offset, Id);
        }
        /// <summary>
        /// Writing Variable Type on the buffer
        /// </summary>
        /// <param name="buffer">Buffer on which value is to be written</param>
        /// <param name="offset">offset on which the value is to be written in buffer</param>
        private void WriteVariableTypeOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteVariableType(ref offset, VariableType);
        }
    }
}
