﻿using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal abstract class BaseVariable
    {
        public ushort Id { get; private set; }
        public string Name { get; private set; }

        protected static int IdSizeOnBuffer = sizeof(short);

        public List<VariableProvider> Providers { get; set; } = new();

        public DateTime LastUpdatedAt { get; set; } = DateTime.Now;

        public BaseVariable(ushort id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Total size => [ID]-[Type]-[Value]
        /// </summary>
        /// <returns></returns>
        public abstract int GetSizeOnBuffer();
        /// <summary>
        /// Write everything including ID, Type and Value
        /// </summary>
        /// <param name="buffer">Buffer on which to write</param>
        /// <param name="offset">Offset in buffer - also updated after writing</param>
        public abstract void WriteOnBuffer(ref byte[] buffer, ref int offset);
        /// <summary>
        /// Writing ID on the buffer
        /// </summary>
        /// <param name="buffer">Buffer on which value is to be written</param>
        /// <param name="offset">offset on which the value is to be written in buffer</param>
        protected void WriteIdOnBuffer(ref byte[] buffer, ref int offset)
        {
            buffer.WriteUnsignedWord(ref offset, Id);
        }
    }
}
