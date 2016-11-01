using System;
using System.Linq;
using System.Text;
using EnvDTE;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.Templates
{
    public class InRoleValidationRuleTemplate : Template
    {
        private static readonly string TemplateContent = Files.InRoleValidationRuleTemplate;

        public InRoleValidationRuleTemplate()
            : base("InRoleValidationRule", TemplateContent)
        {

        }
    }
}