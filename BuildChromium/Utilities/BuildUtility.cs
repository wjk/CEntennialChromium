using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

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
    }
}
