using System.Linq;
using Slalom.Boost.VisualStudio;

namespace Slalom.Boost.Templates
{
    public class TemplateController
    {
        //public Template GetCurrent()
        //{
        //    var text = Application.Current.ActiveDocument.GetText();

        //    var template = Application.Current.Container.ResolveAll<Template>().FirstOrDefault(e => e.FileIdentifierRegex.IsMatch(text));

        //    return template;
        //}

        public Template GetTemplate(string template)
        {
            return Application.Current.Container.ResolveAll<Template>().FirstOrDefault(e => e.Name == template);
        }
    }
}