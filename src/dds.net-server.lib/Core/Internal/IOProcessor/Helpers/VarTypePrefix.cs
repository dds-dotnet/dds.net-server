using DDS.Net.Server.Core.Internal.IOProcessor.Types;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Helpers
{
    internal static class VarTypePrefix
    {
        public static string VarNamePrefix(this VarType varType)
        {
            return varType switch
            {
                VarType.String =>        "_str_",

                VarType.Boolean =>       "_bool_",

                VarType.Byte =>          "_byte_",
                VarType.Word =>          "_word_",
                VarType.DWord =>         "_dword_",
                VarType.QWord =>         "_qword_",

                VarType.UnsignedByte =>  "_ubyte_",
                VarType.UnsignedWord =>  "_uword_",
                VarType.UnsignedDWord => "_udword_",
                VarType.UnsignedQWord => "_uqword_",

                VarType.Single =>        "_sngl_",
                VarType.Double =>        "_dbl_",

                _ => $"_{varType.ToString()}_",
            };
        }
    }
}
