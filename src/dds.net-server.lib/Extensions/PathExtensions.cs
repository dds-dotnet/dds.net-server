namespace DDS.Net.Server.Extensions
{
    internal static class PathExtensions
    {
        public static List<string> TrimmedParts(this string str, char splitter)
        {
            if (string.IsNullOrEmpty(str) == false)
            {
                string[] parts = str.Split(splitter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 0)
                {
                    return new List<string>(parts);
                }
            }

            return new List<string>();
        }
    }
}
