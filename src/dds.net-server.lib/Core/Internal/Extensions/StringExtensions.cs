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

        public static bool IsContainingAnyIgnoringCase(this string text,
            string text01,
            string text02,
            string text03)
        {
            return text.Contains(text01, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text02, StringComparison.CurrentCultureIgnoreCase) ||
                   text.Contains(text03, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsContainingAnyIgnoringCase(this string text,
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

        public static bool IsContainingAnyIgnoringCase(this string text,
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
