using System.Text.RegularExpressions;

namespace DDS.Net.Server.Core.Internal.Extensions
{
    internal static class StringExtensions
    {
        private static Regex spacesPattern = new Regex(@"\s*");

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) ||
                   string.IsNullOrWhiteSpace(value);
        }

        public static string RemoveSpaces(this string value)
        {
            return spacesPattern.Replace(value, "");
        }

        public static bool ContainsAnyIgnoringCase(this string text,
            string text01,
            string text02)
        {
            return text.Contains(text01, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text02, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool ContainsAnyIgnoringCase(this string text,
            string text01,
            string text02,
            string text03)
        {
            return text.Contains(text01, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text02, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text03, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool ContainsAnyIgnoringCase(this string text,
            string text01,
            string text02,
            string text03,
            string text04)
        {
            return text.Contains(text01, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text02, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text03, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text04, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool ContainsAnyIgnoringCase(this string text,
            string text01,
            string text02,
            string text03,
            string text04,
            string text05)
        {
            return text.Contains(text01, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text02, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text03, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text04, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text05, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool ContainsAnyIgnoringCase(this string text,
            string text01,
            string text02,
            string text03,
            string text04,
            string text05,
            params string[] textContd)
        {
            bool contains =
                   text.Contains(text01, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text02, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text03, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text04, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text05, StringComparison.CurrentCultureIgnoreCase);

            if (contains)
            {
                return true;
            }
            else
            {
                foreach (var item in textContd)
                {
                    if (text.Contains(item, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
