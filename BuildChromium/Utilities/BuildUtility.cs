using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using BuildChromium.Logging;

namespace BuildChromium.Utilities
{
    public static class BuildUtility
    {
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
    }
}
