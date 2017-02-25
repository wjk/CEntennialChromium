using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace BuildChromium.Utilities
{
    public static class ExtensionMethods
    {
        public static XElement Element(this XElement parent, string localName) => parent.Element(XName.Get(localName, ""));
        public static XAttribute Attribute(this XElement element, string localName) => element.Attribute(XName.Get(localName, ""));
    }
}
