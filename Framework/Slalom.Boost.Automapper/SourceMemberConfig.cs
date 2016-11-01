using System.Reflection;

namespace Slalom.Boost.AutoMapper
{
    /// <summary>
    /// Contains member configuration relating to source members
    /// </summary>
    public class SourceMemberConfig
    {
        private bool _ignored;

        public SourceMemberConfig(MemberInfo sourceMember)
        {
            this.SourceMember = sourceMember;
        }

        public MemberInfo SourceMember { get; private set; }

        public void Ignore()
        {
            _ignored = true;
        }

        public bool IsIgnored()
        {
            return _ignored;
        }
    }
}