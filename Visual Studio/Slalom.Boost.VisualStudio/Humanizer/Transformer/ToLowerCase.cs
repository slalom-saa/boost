using System.Globalization;

namespace Slalom.Boost.VisualStudio.Humanizer.Transformer
{
    class ToLowerCase : IStringTransformer
    {
        public string Transform(string input)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToLower(input);
        }
    }
}