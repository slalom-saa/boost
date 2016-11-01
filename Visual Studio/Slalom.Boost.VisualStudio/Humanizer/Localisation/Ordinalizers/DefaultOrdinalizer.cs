﻿namespace Slalom.Boost.VisualStudio.Humanizer.Localisation.Ordinalizers
{
    internal class DefaultOrdinalizer : IOrdinalizer
    {
        public virtual string Convert(int number, string numberString, GrammaticalGender gender)
        {
            return this.Convert(number, numberString);
        }

        public virtual string Convert(int number, string numberString)
        {
            return numberString;
        }
    }
}