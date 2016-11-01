using System;
using System.IO;
using System.Text.RegularExpressions;
using EnvDTE;

namespace Slalom.Boost.VisualStudio.IDE
{
    public class ProjectItemDescriptor
    {
        private readonly Lazy<ClassItemDescriptor> _descriptor;
        private string _content;

        public ProjectItemDescriptor(SelectedItem dte)
        {
            this.ProjectDTE = dte;

            _descriptor = new Lazy<ClassItemDescriptor>(() => null);
        }

        public ProjectItemDescriptor(EnvDTE.ProjectItem dte)
        {
            this.DTE = dte;

            _descriptor = new Lazy<ClassItemDescriptor>(() => this.GetClassItemDescriptor(dte));
        }

        public SelectedItem ProjectDTE { get; set; }

        public bool IsFolder => this.DTE?.Kind == "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";

        public bool IsProject => this.ProjectDTE != null;

        public bool IsAggregateRoot => this.IsFile && Regex.IsMatch(this.Content, @"class.*:.*IAggregateRoot");

        public bool IsProcessManager => this.IsFile && Regex.IsMatch(this.Content, @"class.*:.*ProcessManager");

        public EnvDTE.ProjectItem DTE { get; }

        public string Path => this.DTE.FileNames[0];

        public string Content => _content = _content ?? File.ReadAllText(this.Path);

        public ClassItemDescriptor ClassDescriptor => _descriptor.Value;

        public Project Project => this.DTE.ContainingProject;

        public bool IsFile => this.DTE.Kind == "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}";

        public bool IsCommand => this.IsFile && Regex.IsMatch(this.Content, @":\s*Command\s|:\sCommand<");

        public bool IsInputValidation => this.IsFile && Regex.IsMatch(this.Content, @"(:\s*InputValidationRuleSet<\S*>)");

        public bool IsBusinessValidation => this.IsFile && Regex.IsMatch(this.Content, @"(:\s*BusinessValidationRule<\S*>)");

        public string CommandName => this.IsCommand ? System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(this.Path)) : null;

        public bool IsCommandHandler => this.IsFile && Regex.IsMatch(this.Content, @"(:\s*CommandHandler<\S*>)");

        public bool IsEntity => this.IsFile && Regex.IsMatch(this.Content, @":\s*Entity\s|:\s*IEntity");

        public string ClassName => this.IsFile ? Regex.Match(this.Content, @"\sclass\s*(\S*)").Groups[1].Value : null;

        public bool IsReadModel => this.IsFile && Regex.IsMatch(this.Content, @":\s*IReadModel");

        public string Name => this.DTE.Name;

        public Solution Solution => this.DTE.DTE.Solution;

        public bool IsEventSource => this.IsFile && Regex.IsMatch(this.Content, @":\s*EventSource\s|:\s*IEventSource");

        public bool Exists => this.DTE != null;

        private ClassItemDescriptor GetClassItemDescriptor(EnvDTE.ProjectItem dte)
        {
            try
            {
                if (this.IsFile && dte.FileCodeModel != null)
                {
                    return new ClassItemDescriptor(this);
                }
            }
            catch
            {
            }
            return null;
        }

        public void Open()
        {
            this.DTE.Open();
            this.DTE.Document.Activate();
        }
    }
}