using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.WpfApp.Extensions
{
    internal static class FileExtensions
    {
        public static void CreateFoldersForRelativeFilename(this string filename)
        {
            if (string.IsNullOrEmpty(filename)) return;

            string[] folders = filename.Split(
                new char[] { '\\', '/' },
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (folders.Length > 1)
            {
                string foldername = "";

                for(int i = 0;  i < folders.Length - 1; i++)
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
