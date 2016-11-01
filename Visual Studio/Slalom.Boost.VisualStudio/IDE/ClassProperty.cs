using System;

namespace Slalom.Boost.VisualStudio.IDE
{
    public class ClassProperty
    {
        public ClassProperty(string name)
            : this(AccessModifier.Public, "string", name)
        {
        }

        public ClassProperty(string propertyType, string name)
            : this(AccessModifier.Public, propertyType, name)
        {
        }

        public ClassProperty(AccessModifier accessModifier, string propertyType, string name)
        {
            this.AccessModifier = accessModifier;
            this.PropertyType = propertyType;
            this.Name = name;
        }

        public AccessModifier AccessModifier { get; }

        public string PropertyType { get; }

        public string Name { get; }

        public static implicit operator ClassProperty(string value)
        {
            return new ClassProperty(value);
        }
    }
}