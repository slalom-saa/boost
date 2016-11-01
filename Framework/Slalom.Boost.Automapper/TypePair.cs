using System;
using System.Diagnostics;

namespace Slalom.Boost.AutoMapper
{
    [DebuggerDisplay("{SourceType.Name}, {DestinationType.Name}")]
    public class TypePair : IEquatable<TypePair>
    {

        public TypePair(Type sourceType, Type destinationType)
        {
            this.SourceType = sourceType;
            this.DestinationType = destinationType;
            _hashcode = unchecked((this.SourceType.GetHashCode() * 397) ^ this.DestinationType.GetHashCode());
        }

        private readonly int _hashcode;

        public Type SourceType { get; }

        public Type DestinationType { get; }

        public bool Equals(TypePair other)
        {
            return Equals(other.SourceType, this.SourceType) && Equals(other.DestinationType, this.DestinationType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(TypePair)) return false;
            return this.Equals((TypePair)obj);
        }

        public override int GetHashCode()
        {
            return _hashcode;
        }
    }
}
