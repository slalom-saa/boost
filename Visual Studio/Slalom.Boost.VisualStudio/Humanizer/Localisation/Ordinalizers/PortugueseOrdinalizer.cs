﻿namespace Slalom.Boost.VisualStudio.Humanizer.Localisation.Ordinalizers
{
    internal class PortugueseOrdinalizer : DefaultOrdinalizer
    {
        public override string Convert(int number, string numberString)
        {
            return this.Convert(number, numberString, GrammaticalGender.Masculine);
        }

        public override string Convert(int number, string numberString, GrammaticalGender gender)
        {
            // N/A in Portuguese
            if (number == 0)
                return "0";

            if (gender == GrammaticalGender.Feminine)
                return numberString + "ª";

            return numberString + "º";
        }
    }
}
