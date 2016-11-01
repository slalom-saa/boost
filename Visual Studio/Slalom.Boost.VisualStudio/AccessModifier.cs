using System;
using EnvDTE;

namespace Slalom.Boost.VisualStudio
{
    public enum AccessModifier
    {
        Public
    }

    public static class AccessModifierExtensions
    {
        public static string GetSystemName(this AccessModifier instance)
        {
            switch (instance)
            {
                case AccessModifier.Public:
                    return "public";
                default:
                    throw new ArgumentOutOfRangeException(nameof(instance), instance, null);
            }
        }
    }
}