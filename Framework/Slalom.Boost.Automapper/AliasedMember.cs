namespace Slalom.Boost.AutoMapper
{
    public class AliasedMember
    {
        public AliasedMember(string member, string alias)
        {
            this.Member = member;
            this.Alias = alias;
        }

        public string Member { get; }
        public string Alias { get; }

        public bool Equals(AliasedMember other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Member, this.Member) && Equals(other.Alias, this.Alias);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (AliasedMember)) return false;
            return this.Equals((AliasedMember) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Member.GetHashCode()*397) ^ this.Alias.GetHashCode();
            }
        }
    }
}