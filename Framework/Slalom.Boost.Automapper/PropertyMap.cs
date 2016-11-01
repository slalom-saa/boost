using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Slalom.Boost.AutoMapper.Internal;

namespace Slalom.Boost.AutoMapper
{
    [DebuggerDisplay("{DestinationProperty.Name}")]
    public class PropertyMap
    {
        private readonly LinkedList<IValueResolver> _sourceValueResolvers = new LinkedList<IValueResolver>();
        private bool _ignored;
        private int _mappingOrder;
        private IValueResolver _customResolver;
        private IValueResolver _customMemberResolver;
        private bool _sealed;
        private IValueResolver[] _cachedResolvers;
        private Func<ResolutionContext, bool> _condition;
        private Func<ResolutionContext, bool> _preCondition;
        private MemberInfo _sourceMember;

        public PropertyMap(IMemberAccessor destinationProperty)
        {
            this.UseDestinationValue = true;
            this.DestinationProperty = destinationProperty;
        }

        public PropertyMap(PropertyMap inheritedMappedProperty)
            : this(inheritedMappedProperty.DestinationProperty)
        {
            if (inheritedMappedProperty.IsIgnored())
                this.Ignore();
            else
            {
                foreach (var sourceValueResolver in inheritedMappedProperty.GetSourceValueResolvers())
                {
                    this.ChainResolver(sourceValueResolver);
                }
            }
            this.ApplyCondition(inheritedMappedProperty._condition);
            this.SetNullSubstitute(inheritedMappedProperty.NullSubstitute);
            this.SetMappingOrder(inheritedMappedProperty._mappingOrder);
            this.CustomExpression = inheritedMappedProperty.CustomExpression;
        }

        public IMemberAccessor DestinationProperty { get; }

        public Type DestinationPropertyType => this.DestinationProperty.MemberType;

        public LambdaExpression CustomExpression { get; private set; }

        public MemberInfo SourceMember
        {
            get
            {
                return _sourceMember ?? this.GetSourceValueResolvers().OfType<IMemberGetter>().LastOrDefault()?.MemberInfo;
            }
            internal set { _sourceMember = value; }
        }

        public bool CanBeSet => !(this.DestinationProperty is PropertyAccessor) ||
                                ((PropertyAccessor) this.DestinationProperty).HasSetter;

        public bool UseDestinationValue { get; set; }

        internal bool HasCustomValueResolver { get; private set; }

        public bool ExplicitExpansion { get; set; }

        public object NullSubstitute { get; private set; }

        public IEnumerable<IValueResolver> GetSourceValueResolvers()
        {
            if (_customMemberResolver != null)
                yield return _customMemberResolver;

            if (_customResolver != null)
                yield return _customResolver;

            foreach (var resolver in _sourceValueResolvers)
            {
                yield return resolver;
            }

            if (this.NullSubstitute != null)
                yield return new NullReplacementMethod(this.NullSubstitute);
        }

        public void RemoveLastResolver()
        {
            _sourceValueResolvers.RemoveLast();
        }

        public ResolutionResult ResolveValue(ResolutionContext context)
        {
            this.Seal();

            var result = new ResolutionResult(context);

            return _cachedResolvers.Aggregate(result, (current, resolver) => resolver.Resolve(current));
        }

        internal void Seal()
        {
            if (_sealed)
            {
                return;
            }

            _cachedResolvers = this.GetSourceValueResolvers().ToArray();
            _sealed = true;
        }

        public void ChainResolver(IValueResolver valueResolver)
        {
            _sourceValueResolvers.AddLast(valueResolver);
        }

        public void AssignCustomExpression(LambdaExpression customExpression)
        {
            this.CustomExpression = customExpression;
        }

        public void AssignCustomValueResolver(IValueResolver valueResolver)
        {
            _ignored = false;
            _customResolver = valueResolver;
            this.ResetSourceMemberChain();
            this.HasCustomValueResolver = true;
        }

        public void ChainTypeMemberForResolver(IValueResolver valueResolver)
        {
            this.ResetSourceMemberChain();
            _customMemberResolver = valueResolver;
        }

        public void ChainConstructorForResolver(IValueResolver valueResolver)
        {
            _customResolver = valueResolver;
        }

        public void Ignore()
        {
            _ignored = true;
        }

        public bool IsIgnored()
        {
            return _ignored;
        }

        public void SetMappingOrder(int mappingOrder)
        {
            _mappingOrder = mappingOrder;
        }

        public int GetMappingOrder()
        {
            return _mappingOrder;
        }

        public bool IsMapped()
        {
            return _sourceValueResolvers.Count > 0 || this.HasCustomValueResolver || _ignored;
        }

        public bool CanResolveValue()
        {
            return (_sourceValueResolvers.Count > 0 || this.HasCustomValueResolver) && !_ignored;
        }

        public void SetNullSubstitute(object nullSubstitute)
        {
            this.NullSubstitute = nullSubstitute;
        }

        private void ResetSourceMemberChain()
        {
            _sourceValueResolvers.Clear();
        }

        public bool Equals(PropertyMap other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.DestinationProperty, this.DestinationProperty);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (PropertyMap)) return false;
            return this.Equals((PropertyMap) obj);
        }

        public override int GetHashCode()
        {
            return this.DestinationProperty.GetHashCode();
        }

        public void ApplyCondition(Func<ResolutionContext, bool> condition)
        {
            _condition = condition;
        }

        public void ApplyPreCondition(Func<ResolutionContext, bool> condition)
        {
            _preCondition = condition;
        }

        public bool ShouldAssignValue(ResolutionContext context)
        {
            return _condition == null || _condition(context);
        }

        public bool ShouldAssignValuePreResolving(ResolutionContext context)
        {
            return _preCondition == null || _preCondition(context);
        }

        public void SetCustomValueResolverExpression<TSource, TMember>(Expression<Func<TSource, TMember>> sourceMember)
        {
            var body = sourceMember.Body as MemberExpression;
            if (body != null)
            {
                this.SourceMember = body.Member;
            }
            this.CustomExpression = sourceMember;
            this.AssignCustomValueResolver(
                new NullReferenceExceptionSwallowingResolver(
                    new DelegateBasedResolver<TSource, TMember>(sourceMember.Compile())
                    )
                );
        }

        public object GetDestinationValue(object mappedObject)
        {
            return this.UseDestinationValue
                ? this.DestinationProperty.GetValue(mappedObject)
                : null;
        }
    }
}