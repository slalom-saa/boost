using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EnvDTE;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.Humanizer;

namespace Slalom.Boost.Templates
{
    public static class CodePropertyExtensions
    {
        public static string GetDefaultConstructorArguments(this IEnumerable<CodeProperty> properties)
        {
            return properties.GetDefaultConstructorArguments(new string[0]);
        }

        public static string GetDefaultConstructorArguments(this IEnumerable<CodeProperty> properties, params string[] exclude)
        {
            return string.Join(", ", properties.Where(e => !exclude.Contains(e.Name)).Select(e => $"default({e.Type.AsFullName})"));
        }

        public static string GetParameterComments(this IEnumerable<CodeProperty> properties, params string[] exclude)
        {
            var builder = new StringBuilder();
            foreach (var item in properties.Where(e => !exclude.Contains(e.Name)))
            {
                builder.AppendLine();
                builder.Append($"\t\t/// <param name=\"{item.Name.ToCamelCase()}\">The {item.Name.Humanize(LetterCasing.LowerCase)}.</param>");
            }
            return builder.ToString();
        }

        public static string GetParameterComments(this IEnumerable<CodeProperty> properties)
        {
            return properties.GetParameterComments(new string[0]);
        }

        public static string GetPropertyArguments(this IEnumerable<CodeProperty> properties, params string[] exclude)
        {
            return string.Join(", ", properties.Where(e => !exclude.Contains(e.Name)).Select(e => $"{e.GetTypeName()} {e.Name.ToCamelCase()}"));
        }

        public static string GetPropertyArguments(this IEnumerable<CodeProperty> properties)
        {
            return properties.GetPropertyArguments(new string[0]);
        }

        public static string GetPropertyAssignments(this IEnumerable<CodeProperty> properties, params string[] exclude)
        {
            return GetPropertyAssignments(properties, true, exclude);
        }

        public static string GetPropertyAssignments(this IEnumerable<CodeProperty> properties, bool hasSetter, params string[] exclude)
        {
            return string.Join("\r\n", properties.Where(e => !exclude.Contains(e.Name)).Select(e => $"\t\t\tthis.{e.Name} = {e.Name.ToCamelCase()};"));
        }

        public static string GetPropertyAssignments(this IEnumerable<CodeProperty> properties)
        {
            return properties.GetPropertyAssignments(new string[0]);
        }

        public static string GetPropertyDeclaration(this IEnumerable<CodeProperty> properties)
        {
            return properties.GetPropertyDeclaration(new string[0]);
        }

        public static string GetPropertyDeclaration(this IEnumerable<CodeProperty> properties, params string[] exclude)
        {
            return GetPropertyDeclaration(properties, true, exclude);
        }

        private static string GetTypeName(this CodeProperty property)
        {
            try
            {
                var concept = property.Type.CodeType?.Bases.OfType<CodeElement>().FirstOrDefault(e => e.Name == "ConceptAs");
                if (concept != null)
                {
                    var type = Type.GetType(Regex.Match(concept.FullName, "ConceptAs<(.*)>").Groups[1].Value);
                    return GetFriendlyName(type);
                }
            }
            catch
            {
            }
            return property.Type.AsString;
        }

        private static string GetFriendlyName(Type type)
        {
            var codeDomProvider = CodeDomProvider.CreateProvider("C#");
            var typeReferenceExpression = new CodeTypeReferenceExpression(new CodeTypeReference(type));
            using (var writer = new StringWriter())
            {
                codeDomProvider.GenerateCodeFromExpression(typeReferenceExpression, writer, new CodeGeneratorOptions());
                return writer.GetStringBuilder().ToString();
            }
        }

        public static string GetPropertyDeclaration(this IEnumerable<CodeProperty> properties, bool hasSetter, params string[] exclude)
        {
            var codeProperties = properties as IList<CodeProperty> ?? properties.ToList();
          
            var setter = hasSetter ? "private " : "";
            return (codeProperties.Any() ? "\r\n" : "") + string.Join("\r\n\r\n", codeProperties.Where(e => !exclude.Contains(e.Name)).Select(e => $"\t\t{GetPropertyComment(e.Name)}\t\tpublic {e.GetTypeName()} {e.Name} {{ get; {setter}set; }}"));
        }

        public static string GetPublicPropertyDeclaration(this IEnumerable<CodeProperty> properties)
        {
            return properties.GetPublicPropertyDeclaration(new string[0]);
        }

        public static string GetPublicPropertyDeclaration(this IEnumerable<CodeProperty> properties, params string[] exclude)
        {
            var codeProperties = properties as IList<CodeProperty> ?? properties.ToList();

            return (codeProperties.Any() ? "\r\n" : "") + string.Join("\r\n\r\n", codeProperties.Where(e => !exclude.Contains(e.Name)).Select(e => $"\t\t{GetPropertyComment(e.Name)}\t\tpublic {e.GetTypeName()} {e.Name} {{ get; set; }}"));
        }

        public static string GetPropertyInitializer(this IEnumerable<CodeProperty> properties, params string[] exclude)
        {
            return string.Join(",\r\n", properties.Where(e => !exclude.Contains(e.Name)).Select(e => $"\t\t\t\t{e.Name} = {e.Name.ToCamelCase()}"));
        }

        public static string GetPropertyInitializer(this IEnumerable<CodeProperty> properties)
        {
            return properties.GetPropertyInitializer(new string[0]);
        }

        public static string GetPropertyParameters(this IEnumerable<CodeProperty> properties)
        {
            return properties.GetPropertyParameters(null);
        }

        public static string GetPropertyParameters(this IEnumerable<CodeProperty> properties, string initial)
        {
            var codeProperties = properties as IList<CodeProperty> ?? properties.ToList();
            var content = initial ?? "";
            return content + string.Join(", ", codeProperties.Select(e => $"{e.Name.ToCamelCase()}"));
        }

        private static string GetPropertyComment(string name, bool hasSetter = true)
        {
            var setter = hasSetter ? "or sets " : "";
            return $@"/// <summary>
        /// Gets {setter}the [value].
        /// </summary>
        /// <value>
        /// The [value].
        /// </value>".Replace("[value]", name.Humanize(LetterCasing.LowerCase)) + "\r\n";
        }
    }
}