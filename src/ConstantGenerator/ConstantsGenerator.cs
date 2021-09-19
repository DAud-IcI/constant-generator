using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Generator;
using Microsoft.CodeAnalysis;

namespace NotifyPropertyChangedGenerator
{
    [Generator]
    public class ConstantsGenerator : ISourceGenerator
    {
        public const string ConstantXmlExtension = ".constants.xml";
        public const string MetaDataXmlNamespace = "https://github.com/DAud-IcI/constant-generator/";
        
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            foreach (var constantFile in context.AdditionalFiles.Where(x => x.Path.EndsWith(ConstantXmlExtension)))
            {
                var fileName = Path.GetFileName(constantFile.Path);
                var name = fileName[..ConstantXmlExtension.Length];

                var xml = XDocument.Parse(File.ReadAllText(constantFile.Path));
                var root = xml.Root ?? throw new InvalidOperationException("Root can't be null.");

                var xmlContext = new XmlContext(
                    root
                        .Elements()
                        .Where(element => element.Name.Namespace == MetaDataXmlNamespace));

                var sb = new StringBuilder($"namespace {xmlContext.Namespace}\n{{\n");
                Generate(sb, root, xmlContext);
                sb.AppendLine("}");

                context.AddSource(name + ".cs", sb.ToString());
            }
        }

        private void Generate(StringBuilder sb, XElement element, XmlContext context)
        {
            if (element.Name.Namespace == MetaDataXmlNamespace) return; // Skip meta elements.

            context = context.Descend(element.Name.LocalName);
            var name = element.Name.LocalName;
            var indentation = context.Indentation;
            
            if (!element.HasElements)
            {
                sb.AppendLine($"{indentation}public const string {name} = \"{context.Path}\";");
                return;
            }

            sb.AppendLine($"\n{indentation}public static class {name}\n{indentation}{{");
            foreach (var child in element.Elements())
            {
                Generate(sb, child, context);
            }
            sb.AppendLine($"{indentation}}}");
        }
    }
}