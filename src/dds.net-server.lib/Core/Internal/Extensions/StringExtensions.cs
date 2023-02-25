namespace DDS.Net.Server.Core.Internal.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) ||
                   string.IsNullOrWhiteSpace(value);
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
    }
}
