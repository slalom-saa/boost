using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
using Slalom.Boost.Extensions;
using Slalom.Boost.Runtime.Humanizer;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files
{
    public class ProcessManagerCommandBuilder : ClassBuilder
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

        public ProcessManagerCommandBuilder(string name, string verb, IEnumerable<string> properties, ProjectItemDescriptor selectedItem)
            : base(Templates.ProcessManagerCommand, selectedItem)
        {
            this.Name = name;
            this.Verb = verb;
            this.RelativePath = $"{name.Pluralize()}\\{verb}\\{verb}Command.cs";
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

        public ProcessManagerCommandBuilder(string name, string verb, IEnumerable<Property> properties, ProjectItemDescriptor selectedItem)
            : base(Templates.ProcessManagerCommand, selectedItem)
        {
            this.Name = name;
            this.Verb = verb;
            this.RelativePath = $"{name.Pluralize()}\\{verb}\\{verb}Command.cs";
            this.Properties = (properties.Any() ? "\r\n" : "") + string.Join("\r\n\r\n", properties.Select(e => $"\t\t{GetPropertyComment(e.Name)}\t\tpublic {e.PropertyType} {e.Name} {{ get; private set; }}"));
            this.PropertyArguments = string.Join(", ", properties.Select(e => $"{e.PropertyType} {e.Name.ToCamelCase()}"));
            this.PropertyAssignments = string.Join("\r\n", properties.Select(e => $"\t\t\tthis.{e.Name} = {e.Name.ToCamelCase()};"));
            this.ParamString = "";
            foreach (var item in properties)
            {
                this.ParamString += "\r\n";
                this.ParamString += $"\t\t/// <param name=\"{item.Name.ToCamelCase()}\">The {item.Name.Humanize(LetterCasing.LowerCase)}.</param>";
            }
        }

        public string Verb { get; set; }

        public string ParamString { get; set; }

        public string Name { get; }

        public string PropertyAssignments { get; set; }

        public string PropertyArguments { get; set; }

        public string Properties { get; set; }
    }
}