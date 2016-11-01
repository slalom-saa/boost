using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Slalom.Boost.VisualStudio.IDE
{
    //public class ProjectItem2
    //{
    //    public static implicit operator ProjectItem(ProjectItem item)
    //    {
    //        return item.ProjectItem;
    //    }

    //    private readonly Lazy<string> _content;

    //    public EnvDTE.ProjectItem DTE { get; }

    //    public ProjectItem(EnvDTE.ProjectItem dte)
    //    {
    //        this.DTE = dte;

    //        _content = new Lazy<string>(() => File.ReadAllText(this.DTE.ProjectItem.FileNames[0]));
    //    }

    //    public ProjectItem ProjectItem => new ProjectItem(this.DTE.ProjectItem ?? this.DTE.Project.ProjectItems.Item(1));

    //    public bool IsFile => this.DTE.ProjectItem?.Kind == "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}";

    //    public bool IsCommand => this.IsFile && Regex.IsMatch(this.Content, @":\s*Command\s|:\sCommand<");

    //    public bool IsInputValidation => this.IsFile && Regex.IsMatch(this.Content, @"(:\s*InputValidationRuleSet<\S*>)");

    //    public bool IsBusinessValidation => this.IsFile && Regex.IsMatch(this.Content, @"(:\s*BusinessValidationRule<\S*>)");

    //    public string CommandName => this.IsCommand ? System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(this.Path)) : null;

    //    public bool IsCommandHandler => this.IsFile && Regex.IsMatch(this.Content, @"(:\s*CommandHandler<\S*>)");

    //    public bool IsEntity => this.IsFile && Regex.IsMatch(this.Content, @"class.*:.*Entity");

    //    public bool IsAggregateRoot  => this.IsFile && Regex.IsMatch(this.Content, @"class.*:.*IAggregateRoot");

    //    public string ClassName => this.IsFile ? Regex.Match(this.Content, @"\sclass\s*(\S*)").Groups[1].Value : null;

    //    public bool IsReadModel => this.IsFile && Regex.IsMatch(this.Content, @":\s*IReadModel");

    //    public bool IsFolder => this.DTE.ProjectItem?.Kind == "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";

    //    public bool IsProject => this.DTE.Project != null;

    //    public string Name => this.DTE.Name;

    //    public Project Project => new Project(this.DTE.Project ?? this.DTE.ProjectItem.ContainingProject);

    //    public string Path => this.DTE.ProjectItem.FileNames[0];

    //    public Solution Solution => new Solution(this.DTE.DTE.Solution);

    //    public string Content => _content.Value;

    //    public ClassItemDescriptor ClassDescriptor => this.ProjectItem.ClassDescriptor;

    //    public bool IsEventSource => this.IsFile && Regex.IsMatch(this.Content, @"class.*:.*EventSource");

    //    public bool IsProcessManager => this.IsFile && Regex.IsMatch(this.Content, @"class.*:.*ProcessManager");
    //}
}