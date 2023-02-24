using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDS.Net.Server.WpfApp.Extensions
{
    internal static class FileExtensions
    {
        public static void CreateFoldersForFile(this string filename)
        {
            if (string.IsNullOrEmpty(filename)) return;

            string[] folders = filename.Split(
                new char[] { '\\', '/' },
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
