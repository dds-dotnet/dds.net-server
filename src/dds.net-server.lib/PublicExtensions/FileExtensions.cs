namespace DDS.Net.Server.PublicExtensions
{
    public static class FileExtensions
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
