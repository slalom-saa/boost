using System;
using System.Linq;
using System.Reflection;

namespace Slalom.Boost.AutoMapper.QueryableExtensions
{
    public class ExpressionRequest : IEquatable<ExpressionRequest>
    {
        public Type SourceType { get; }

        public Type DestinationType { get; }

        public MemberInfo[] MembersToExpand { get; }

        public ExpressionRequest(Type sourceType, Type destinationType, params MemberInfo[] membersToExpand)
        {
            this.SourceType = sourceType;
            this.DestinationType = destinationType;
            this.MembersToExpand = membersToExpand.OrderBy(p=>p.Name).ToArray();
        }

        public bool Equals(ExpressionRequest other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.MembersToExpand.SequenceEqual(other.MembersToExpand) &&
                   this.SourceType == other.SourceType && this.DestinationType == other.DestinationType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((ExpressionRequest) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.SourceType.GetHashCode();
                hashCode = (hashCode*397) ^ this.DestinationType.GetHashCode();
                return this.MembersToExpand.Aggregate(hashCode, (currentHash, p) => (currentHash * 397) ^ p.GetHashCode());
            }
        }

        public static bool operator ==(ExpressionRequest left, ExpressionRequest right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ExpressionRequest left, ExpressionRequest right)
        {
            return !Equals(left, right);
        }
    }
}