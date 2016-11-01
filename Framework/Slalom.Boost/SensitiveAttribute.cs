using System;
using System.Linq;

namespace Slalom.Boost
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SensitiveAttribute : Attribute
    {
    }
}
