using System;

namespace Slalom.Boost.AutoMapper
{
    /// <summary>
    /// Represents the result of resolving a value
    /// </summary>
    public class ResolutionResult
    {
        /// <summary>
        /// Create a resolution result based on source values of a resolution context
        /// </summary>
        /// <param name="context">Resolution context</param>
        public ResolutionResult(ResolutionContext context)
            : this(context.SourceValue, context, context.SourceType)
        {
        }

        private ResolutionResult(object value, ResolutionContext context, Type memberType)
        {
            this.Value = value;
            this.Context = context;
            this.Type = ResolveType(value, memberType);
            this.MemberType = memberType;
        }

        private ResolutionResult(object value, ResolutionContext context)
        {
            this.Value = value;
            this.Context = context;
            this.Type = ResolveType(value, typeof (object));
            this.MemberType = this.Type;
        }

        /// <summary>
        /// Resultant value
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Type of value resolved
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Type of member, in case the value is null
        /// </summary>
        public Type MemberType { get; }

        /// <summary>
        /// Context for resolving this value
        /// </summary>
        public ResolutionContext Context { get; }

        /// <summary>
        /// Directs mappers to ignore this value
        /// </summary>
        public bool ShouldIgnore { get; set; }

        private static Type ResolveType(object value, Type memberType)
        {
            return value == null ? memberType : value.GetType();
        }

        /// <summary>
        /// Create a new resolution result representing ignoring the value
        /// </summary>
        /// <returns>New resolution result based on this context with ignored value</returns>
        public ResolutionResult Ignore()
        {
            return new ResolutionResult(this.Context) {ShouldIgnore = true};
        }

        /// <summary>
        /// Create a new resolution result representing the value provided
        /// </summary>
        /// <param name="value">Resolved value</param>
        /// <returns>Resolution result containing resolved value</returns>
        public ResolutionResult New(object value)
        {
            return new ResolutionResult(value, this.Context);
        }

        /// <summary>
        /// Constructs a new resolution result based on the context of this value result
        /// </summary>
        /// <param name="value">Value resolved</param>
        /// <param name="memberType">Type of value as reference in case value is null</param>
        /// <returns>New resolutino result</returns>
        public ResolutionResult New(object value, Type memberType)
        {
            return new ResolutionResult(value, this.Context, memberType);
        }
    }
}