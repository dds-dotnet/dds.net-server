using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.Core.Internal.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsContainingAnyIgnoringCase(this string text,
            string text01,
            string text02)
        {
            return text.Contains(text01, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text02, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
