namespace Slalom.Boost.AutoMapper.Internal
{
    public class MemberNameReplacer
    {
        public MemberNameReplacer(string originalValue, string newValue)
        {
            this.OriginalValue = originalValue;
            this.NewValue = newValue;
        }

        public string OriginalValue { get; private set; }
        public string NewValue { get; private set; }
    }
}