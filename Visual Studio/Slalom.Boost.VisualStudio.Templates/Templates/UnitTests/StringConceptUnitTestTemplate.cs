using System;
using System.Linq;
using EnvDTE;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.Templates
{
    public class StringConceptUnitTestTemplate : Template
    {
        private static readonly string TemplateContent = Files.StringConceptUnitTestTemplate;

        public StringConceptUnitTestTemplate()
            : base("StringConceptUnitTest", TemplateContent)
        {
        }
    }
}