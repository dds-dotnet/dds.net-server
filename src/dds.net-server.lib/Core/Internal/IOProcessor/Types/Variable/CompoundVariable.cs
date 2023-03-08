using DDS.Net.Server.Entities;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Types.Variable
{
    internal class CompoundVariable : BaseVariable
    {
        public List<BaseVariable> VariablesGroup { get; private set; } = new();

        public CompoundVariable(ushort id, string name) : base(id, name)
        {
            VariableType = VariableType.Compound;
        }
        /// <summary>
        /// Adds a variable to the composition / group.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddVariable(BaseVariable variable)
        {
            if (variable == null) { throw new ArgumentNullException(nameof(variable)); }

            VariablesGroup.Add(variable);
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
            //- So, we only return the summed size of internal variables.
            //- 

            int sum = 0;

            foreach (var variable in VariablesGroup)
            {
                sum += variable.GetSizeOnBuffer();
            }

            return sum;
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
            //- So, we only write the internal variables.
            //- 

            foreach (var variable in VariablesGroup)
            {
                variable.WriteValueOnBuffer(ref buffer, ref offset);
            }
        }
    }
}
