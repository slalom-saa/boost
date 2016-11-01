using Slalom.Boost.VisualStudio.Humanizer.Localisation.GrammaticalNumber;

namespace Slalom.Boost.VisualStudio.Humanizer.Localisation.Formatters
{
    internal class RussianFormatter : DefaultFormatter
    {
        public RussianFormatter()
            : base("ru")
        {
        }

        protected override string GetResourceKey(string resourceKey, int number)
        {
            var grammaticalNumber = RussianGrammaticalNumberDetector.Detect(number);
            var suffix = this.GetSuffix(grammaticalNumber);
            return resourceKey + suffix;
        }

        private string GetSuffix(RussianGrammaticalNumber grammaticalNumber)
        {
            if (grammaticalNumber == RussianGrammaticalNumber.Singular)
                return "_Singular";
            if (grammaticalNumber == RussianGrammaticalNumber.Paucal)
                return "_Paucal";
            return "";
        }
    }
}