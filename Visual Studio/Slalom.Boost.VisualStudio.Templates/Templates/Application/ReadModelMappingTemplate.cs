using System;
using System.Linq;
using EnvDTE;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.Templates
{
    public class ReadModelMappingTemplate : Template
    {
        private static readonly string TemplateContent = Files.ReadModelMappingTemplate;

        public ReadModelMappingTemplate()
            : base("ReadModelMapping", TemplateContent)
        {
        }
    }
}