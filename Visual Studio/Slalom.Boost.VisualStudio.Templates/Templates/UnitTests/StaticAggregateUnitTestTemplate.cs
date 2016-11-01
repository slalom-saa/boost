using System;
using System.Linq;
using EnvDTE;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.Templates
{
    public class StaticAggregateUnitTestTemplate : Template
    {
        private static readonly string TemplateContent = Files.StaticAggregateUnitTestTemplate;

        public StaticAggregateUnitTestTemplate()
            : base("StaticAggregateUnitTest", TemplateContent)
        {
        }
    }
}