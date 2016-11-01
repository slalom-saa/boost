using System;

namespace Slalom.Boost.Humanizer.Localisation.Ordinalizers
{
    internal class TurkishOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString)
        {
            return numberString + ".";
        }
    }
}
