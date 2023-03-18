namespace DDS.Net.Server.PublicExtensions
{
    /// <summary>
    /// Provides extension methods for working with files, folders and paths.
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// Always returns a list of strings containing parts of a given
        /// string split by specified character.
        /// </summary>
        /// <param name="str">The input string that needs to be split.</param>
        /// <param name="splitter">The character to split the string over.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates all the folders (if not existing) for putting a file with specified relative path.
        /// </summary>
        /// <param name="filename">Relative path of file.</param>
        public static void CreateFoldersForRelativeFilename(this string filename)
        {
            if (string.IsNullOrEmpty(filename)) return;

            string[] folders = filename.Split(
                new char[] { '\\', '/' },
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (folders.Length > 1)
            {
                string foldername = "";

                for (int i = 0; i < folders.Length - 1; i++)
                {
                    if (foldername == "")
                    {
                        foldername = folders[i];
                    }
                    else
                    {
                        foldername = $"{foldername}{Path.DirectorySeparatorChar}{folders[i]}";
                    }
                }

                Directory.CreateDirectory(foldername);
            }
        }
    }
}
