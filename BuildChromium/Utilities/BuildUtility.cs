using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using BuildChromium.Logging;

namespace BuildChromium.Utilities
{
    public static class BuildUtility
    {
        private static class NativeMethods
        {
            [DllImport("libc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
            public static extern int chmod(string path, int mode);
        }

        public static void PerformSubstitution(IEnumerable<FileInfo> files, List<Tuple<Regex,string>> replacements)
        {
            foreach (var file in files) PerformSubstitution(file, replacements);
        }

        public static void PerformSubstitution(FileInfo file, List<Tuple<Regex, string>> replacements)
        {
            string source;
            using (var reader = file.OpenText())
            {
                source = reader.ReadToEnd();
            }

            foreach (var pair in replacements)
            {
                source = pair.Item1.Replace(source, pair.Item2);
            }

            Stream stream = file.Open(FileMode.Create);
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(source);
            }
        }

        public static void DownloadFile(Uri uri, FileInfo file, bool forceDownload = false)
        {
            if (forceDownload || !file.Exists)
            {
                Log.Write(LogLevel.Informational, "Downloading {0}...", file.FullName);
                WebClient client = new WebClient();
                client.DownloadFile(uri.AbsoluteUri, file.FullName);
            }
            else
            {
                Log.Write(LogLevel.Verbose, "{0} already exists. Skipping download.", file.FullName);
            }
        }

        public static bool Chmod(FileInfo file, int new_mode)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Log.Write(LogLevel.Debug, "Ignoring chmod on Windows, as the concept does not apply there");
                return true;
            }

            int res = NativeMethods.chmod(file.FullName, new_mode);
            return res == 0;
        }
    }
}
