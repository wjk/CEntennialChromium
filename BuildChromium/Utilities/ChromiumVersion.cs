using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace BuildChromium.Utilities
{
    public struct SourceVersion
    {
        public static SourceVersion ParseXml(FileInfo file)
        {
            using (var stream = file.OpenRead())
            {
                XDocument doc = XDocument.Load(stream);
                XElement root = doc.Root.Element(nameof(SourceVersion));

                SourceVersion vers = new SourceVersion();
                vers.ChromiumVersion = root.Element(nameof(vers.ChromiumVersion)).Value;
                vers.ReleaseRevision = root.Element(nameof(vers.ReleaseRevision)).Value;
                return vers;
            }
        }

        public string ChromiumVersion { get; set; }
        public string ReleaseRevision { get; set; }
    }
}
