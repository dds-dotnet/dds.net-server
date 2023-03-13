using DDS.Net.Server.Core.Internal.IOProcessor.EncodersAndDecoders;
using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class RawBytesVariable : BaseVariable
    {
        public byte[] Data { get; private set; }

        public RawBytesVariable(ushort id, string name, byte[] data) : base(id, name)
        {
            VariableType = VariableType.RawBytes;
            Data = data;
        }

        /// <summary>
        /// Updates the data element.
        /// </summary>
        /// <param name="data">New data.</param>
        /// <returns>True = Data is changed; False = Same value as previous</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool UpdateData(byte[] data)
        {
            if (data == null)
            {
                if (Data == null)
                {
                    return false;
                }
                else
                {
                    Data = null!;
                    return true;
                }
            }
            else
            {
                if (Data == null)
                {
                    Data = data;
                    return true;
                }
                else
                {
                    if (Data.Length != data.Length)
                    {
                        Data = data;
                        return true;
                    }
                    else
                    {
                        bool isDiff = false;

                        for (int i = 0; i < Data.Length; i++)
                        {
                            if (Data[i] != data[i])
                            {
                                isDiff = true;
                            }

                            Data[i] = data[i];
                        }

                        return isDiff;
                    }
                }
            }
        }

        public override int GetTypeSizeOnBuffer()
        {
            //- 
            //- We do not have sub-types here unlike the primitive types.
            //- Thereby, its sub-type will take 0-bytes on the data buffer.
            //- 

            return 0;
        }

        public override int GetValueSizeOnBuffer()
        {
            //- 
            //- We do not have sub-types here unlike the primitive types.
            //- Thereby, its sub-type will take 0-bytes on the data buffer.
            //- So, we only return the summed size of internal bytes.
            //- 

            int total = 4; // Number of bytes required for writing array size on buffer

            if (Data != null)
            {
                total += Data.Length;
            }

            return total;
        }

        public override void WriteTypeOnBuffer(ref byte[] buffer, ref int offset)
        {
            //- 
            //- We do not have sub-types here unlike the primitive types.
            //- Thereby, its sub-type will take 0-bytes on the data buffer.
            //- So, we do not write anything here.
            //- 
        }

        public override void WriteValueOnBuffer(ref byte[] buffer, ref int offset)
        {
            //- 
            //- We do not have sub-types here unlike the primitive types.
            //- Thereby, its sub-type will take 0-bytes on the data buffer.
            //- So, we only write the internal data.
            //- 

            if (offset + GetValueSizeOnBuffer() >= buffer.Length)
            {
                throw new Exception(
                    $"Cannot fit {GetValueSizeOnBuffer()} bytes " +
                    $"at offset {offset} on buffer having size {buffer.Length}");
            }

            if (Data != null)
            {
                buffer.WriteUnsignedDWord(ref offset, (uint)Data.Length);

                for (int i = 0; i < Data.Length; i++)
                {
                    buffer[offset++] = Data[i];
                }
            }
            else
            {
                buffer.WriteUnsignedDWord(ref offset, 0); // Size is zero here
            }
        }
    }
}
