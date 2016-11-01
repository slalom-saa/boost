using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
using Slalom.Boost.Extensions;
using Slalom.Boost.Runtime.Humanizer;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files
{
    public class CommandBuilder : ClassBuilder
    {
        private static string GetPropertyComment(string name)
        {
            return @"/// <summary>
        /// Gets the [value].
        /// </summary>
        /// <value>
        /// The [value].
        /// </value>".Replace("[value]", name.Humanize(LetterCasing.LowerCase)) + "\r\n";
        }

        public CommandBuilder(string name, IEnumerable<string> properties, ProjectItemDescriptor selectedItem)
            : base(Templates.Command, selectedItem)
        {
            this.Name = name;
            this.RelativePath = $"{name}\\{name}Command.cs";
            this.Properties = (properties.Any() ? "\r\n" : "") + string.Join("\r\n\r\n", properties.Select(e => $"\t\t{GetPropertyComment(e)}\t\tpublic string {e} {{ get; private set; }}"));
            this.PropertyArguments = string.Join(", ", properties.Select(e => $"string {e.ToCamelCase()}"));
            this.PropertyAssignments = string.Join("\r\n", properties.Select(e => $"\t\t\tthis.{e} = {e.ToCamelCase()};"));
            this.ParamString = "";
            foreach (var item in properties)
            {
                this.ParamString += "\r\n";
                this.ParamString += $"\t\t/// <param name=\"{item.ToCamelCase()}\">The {item.Humanize(LetterCasing.LowerCase)}.</param>";
            }
        }

        public string ParamString { get; set; }

        public string Name { get; }

        public string PropertyAssignments { get; set; }

        public string PropertyArguments { get; set; }

        public string Properties { get; set; }
    }
}