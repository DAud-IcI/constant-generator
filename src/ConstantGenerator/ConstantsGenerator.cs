using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                var name = fileName[..^ConstantXmlExtension.Length];

                var xml = XDocument.Parse(File.ReadAllText(constantFile.Path));
                var root = xml.Root ?? throw new InvalidOperationException("Root can't be null.");

                var xmlContext = new XmlContext(
                    root
                        .Elements()
                        .Where(element => element.Name.Namespace == MetaDataXmlNamespace));

                var lines = new List<string>
                {
                    $"namespace {xmlContext.Namespace}",
                    "{",
                };
                Generate(lines, root, xmlContext);
                lines.Add("}");

                context.AddSource(
                    name.ToPascalCase() + ".GeneratedConstant.cs", 
                    string.Join("\n", lines));
            }
        }

        private void Generate(List<string> lines, XElement element, XmlContext context)
        {
            if (element.Name.Namespace == MetaDataXmlNamespace) return; // Skip meta elements.

            context = context.Descend(element.Name.LocalName);
            var name = element.Name.LocalName;
            var indentation = context.Indentation;
            
            if (!element.HasElements)
            {
                lines.Add($"{indentation}public const string {name} = \"{context.Path}\";");
                return;
            }

            var lastLine = lines[^1].Trim();
            if (!string.IsNullOrEmpty(lastLine) && !lastLine.EndsWith("{")) lines.Add(string.Empty);
                
            lines.Add($"{indentation}public static class {name}");
            lines.Add($"{indentation}{{");
            lines.Add($"{indentation}    public const string ThisRoute = \"{context.Path}\";\n");
            foreach (var child in element.Elements())
            {
                Generate(lines, child, context);
            }
            lines.Add($"{indentation}}}");
        }
    }
}