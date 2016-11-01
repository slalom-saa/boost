namespace Slalom.Boost.Templates
{
    public class BusinessValidationRuleTemplate : Template
    {
        private static readonly string TemplateContent = Files.BusinessValidationRuleTemplate;

        public BusinessValidationRuleTemplate()
            : base("BusinessValidationRule", TemplateContent)
        {
        }
    }
}