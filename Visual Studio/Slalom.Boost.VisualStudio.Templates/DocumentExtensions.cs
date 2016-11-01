using System.IO;
using EnvDTE;

namespace Slalom.Boost.Templates
{
    public static class DocumentExtensions
    {
        public static string GetText(this Document document)
        {
            return File.ReadAllText(document.Path);
        }
    }
}