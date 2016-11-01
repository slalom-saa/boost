using System;
using System.Linq;
using EnvDTE;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.Templates
{
    public class AggregateUnitTestTemplate : Template
    {
        private static readonly string TemplateContent = Files.AggregateUnitTestTemplate;

        public AggregateUnitTestTemplate()
            : base("AggregateUnitTest", TemplateContent)
        {
        }
    }
}