using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ConstantGenerator
{
    public class XmlContext
    {
        public string Namespace { get; } = "Constants";
        public string Separator { get; } = "/";

        public string Path { get; private set; } = "";
        public int Level { get; private set; }
        
        public string Indentation => new(' ', Level * 4);

        private XmlContext(XmlContext original)
        {
            Namespace = original.Namespace;
            Separator = original.Separator;
            Path = original.Path;
            Level = original.Level;
        }

        public XmlContext(IEnumerable<XElement> metaElements)
        {
            foreach (var element in metaElements)
            {
                switch (element.Name.LocalName)
                {
                    case nameof(Namespace): Namespace = element.Attribute("Value")?.Value ?? Namespace; break;
                    case nameof(Separator): Separator = element.Attribute("Value")?.Value ?? Separator; break;
                    default: throw new InvalidOperationException($"Unknown type {element.Name} ({element})");
                }
            }
        }

        public XmlContext Descend(string childName)
        {
            var child = new XmlContext(this);
            child.Path = child.Level == 0 ? childName : child.Path + child.Separator + childName;
            child.Level++;

            return child;
        }
    }
}