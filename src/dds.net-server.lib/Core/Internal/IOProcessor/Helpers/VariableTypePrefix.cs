using DDS.Net.Server.Core.Internal.IOProcessor.Types;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Helpers
{
    internal static class VariableTypePrefix
    {
        public static string VarNamePrefix(this VariableType varType)
        {
            return varType switch
            {
                VariableType.String =>        "_str_",

                VariableType.Boolean =>       "_bool_",

                VariableType.Byte =>          "_byte_",
                VariableType.Word =>          "_word_",
                VariableType.DWord =>         "_dword_",
                VariableType.QWord =>         "_qword_",

                VariableType.UnsignedByte =>  "_ubyte_",
                VariableType.UnsignedWord =>  "_uword_",
                VariableType.UnsignedDWord => "_udword_",
                VariableType.UnsignedQWord => "_uqword_",

                VariableType.Single =>        "_sngl_",
                VariableType.Double =>        "_dbl_",

                _ => $"_{varType}_",
            };
        }
    }
}
