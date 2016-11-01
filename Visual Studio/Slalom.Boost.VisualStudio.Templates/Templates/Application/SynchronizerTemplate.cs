namespace Slalom.Boost.Templates
{
    public class SynchronizerTemplate : Template
    {
        private static readonly string TemplateContent = Files.SynchronizerTemplate;

        public SynchronizerTemplate()
            : base("Synchronizer", TemplateContent)
        {

        }
    }
}