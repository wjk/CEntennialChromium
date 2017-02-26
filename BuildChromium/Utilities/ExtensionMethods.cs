using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace BuildChromium.Utilities
{
    public static class ExtensionMethods
    {
        public static XElement Element(this XElement parent, string localName) => parent.Element(XName.Get(localName, ""));
        public static XAttribute Attribute(this XElement element, string localName) => element.Attribute(XName.Get(localName, ""));

        public static DirectoryInfo GetSubdirectory(this DirectoryInfo parent, string path)
        {
            string subdirPath = Path.Combine(parent.FullName, path);
            if (Directory.Exists(subdirPath)) return new DirectoryInfo(subdirPath);
            else return parent.CreateSubdirectory(path);
        }
    }
}
