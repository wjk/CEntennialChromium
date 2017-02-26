using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BuildChromium.Utilities
{
    public static class ExtensionMethods
    {
        public static XElement Element(this XElement parent, string localName) => parent.Element(XName.Get(localName, ""));
        public static XAttribute Attribute(this XElement element, string localName) => element.Attribute(XName.Get(localName, ""));

        public static FileInfo CreateFile(this DirectoryInfo parent, string fileName)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (fileName.Contains(Path.DirectorySeparatorChar) || fileName.Contains(Path.AltDirectorySeparatorChar)) throw new ArgumentException("File name cannot contain directory separators", nameof(fileName));
            if (fileName.Contains("*") || fileName.Contains("?")) throw new ArgumentException("File name cannot contain wildcard characters", nameof(fileName));

            FileInfo[] existingFiles = parent.GetFiles(fileName);
            if (existingFiles.Length == 1) return existingFiles[0];
            else if (existingFiles.Length > 1) throw new InvalidOperationException($"File name '{fileName}' matched more than one file");

            string fullPath = Path.Combine(parent.FullName, fileName);
            File.WriteAllBytes(fullPath, Array.Empty<byte>());
            return new FileInfo(fullPath);
        }

        public static DirectoryInfo SafeCreateSubdirectory(this DirectoryInfo parent, string path)
        {
            string subdirPath = Path.Combine(parent.FullName, path);
            if (Directory.Exists(subdirPath)) return new DirectoryInfo(subdirPath);
            else return parent.CreateSubdirectory(path);
        }
    }
}
