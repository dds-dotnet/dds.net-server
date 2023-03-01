using DDS.Net.Server.Core.Internal.IOProcessor.Types;

namespace DDS.Net.Server.Core.Internal.IOProcessor.Helpers
{
    internal static class VarTypePrefix
    {
        public static string VarNamePrefix(this VarType varType)
        {
            switch (varType)
            {
                case VarType.String: return "_str_";
                case VarType.Boolean: return "_bool_";
                case VarType.Byte: return "_byte_";
                case VarType.Word: return "_word_";
                case VarType.DWord: return "_dword_";
                case VarType.QWord: return "_qword_";
                case VarType.UnsignedByte: return "_ubyte_";
                case VarType.UnsignedWord: return "_uword_";
                case VarType.UnsignedDWord: return "_udword_";
                case VarType.UnsignedQWord: return "_uqword_";
                case VarType.Single: return "_sngl_";
                case VarType.Double: return "_dbl_";
            }

            return $"_{varType.ToString()}_";
        }
    }
}
