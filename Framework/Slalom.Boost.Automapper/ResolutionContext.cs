using System;
using System.Collections.Generic;
using System.Linq;

namespace Slalom.Boost.AutoMapper
{
    /// <summary>
    /// Context information regarding resolution of a destination value
    /// </summary>
    public class ResolutionContext : IEquatable<ResolutionContext>
    {
        private static readonly ResolutionContext Empty = new ResolutionContext();

        /// <summary>
        /// Mapping operation options
        /// </summary>
        public MappingOperationOptions Options { get; }

        /// <summary>
        /// Current type map
        /// </summary>
        public TypeMap TypeMap { get; }

        /// <summary>
        /// Current property map
        /// </summary>
        public PropertyMap PropertyMap { get; }

        /// <summary>
        /// Initial source type
        /// </summary>
        public Type InitialSourceType { get; }

        /// <summary>
        /// Initial destination type
        /// </summary>
        public Type InitialDestinationType { get; }

        /// <summary>
        /// Current source type
        /// </summary>
        public Type SourceType { get; }

        /// <summary>
        /// Current attempted destination type
        /// </summary>
        public Type DestinationType { get; }

        /// <summary>
        /// Index of current collection mapping
        /// </summary>
        public int? ArrayIndex { get; }

        /// <summary>
        /// Source value
        /// </summary>
        public object SourceValue { get; }

        /// <summary>
        /// Destination value
        /// </summary>
        public object DestinationValue { get; }

        /// <summary>
        /// Parent resolution context
        /// </summary>
        public ResolutionContext Parent { get; }

        /// <summary>
        /// Instance cache for resolving circular references
        /// </summary>
        public Dictionary<ResolutionContext, object> InstanceCache { get; }

        /// <summary>
        /// Current mapping engine
        /// </summary>
        public IMappingEngine Engine { get; }

        /// <summary>
        /// Current configuration
        /// </summary>
        public IConfigurationProvider ConfigurationProvider => this.Engine.ConfigurationProvider;

        /// <summary>
        /// Source and destination type pair
        /// </summary>
        public TypePair Types { get; }


        private ResolutionContext()
        {
        }

        private ResolutionContext(ResolutionContext context, object sourceValue, object destinationValue, Type sourceType, 
            Type destinationType = null, TypeMap typeMap = null)
        {
            if(context != Empty)
            {
                if(context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }
                this.Parent = context;
                this.ArrayIndex = context.ArrayIndex;
                this.PropertyMap = context.PropertyMap;
                this.DestinationType = context.DestinationType;
                this.InstanceCache = context.InstanceCache;
                this.Options = context.Options;
                this.Engine = context.Engine;
            }
            this.SourceValue = sourceValue;
            this.DestinationValue = destinationValue;
            this.InitialSourceType = sourceType;
            this.InitialDestinationType = destinationType;
            this.TypeMap = typeMap;
            if(typeMap != null)
            {
                this.SourceType = typeMap.SourceType;
                this.DestinationType = typeMap.DestinationType;
            }
            else
            {
                this.SourceType = sourceType;
                if(destinationType != null)
                {
                    this.DestinationType = destinationType;
                }
            }
            this.Types = new TypePair(this.SourceType, this.DestinationType);
        }

        public ResolutionContext(TypeMap typeMap, object source, Type sourceType, Type destinationType,
            MappingOperationOptions options, IMappingEngine engine)
            : this(typeMap, source, null, sourceType, destinationType, options, engine)
        {
        }

        public ResolutionContext(TypeMap typeMap, object source, object destination, Type sourceType,
            Type destinationType, MappingOperationOptions options, IMappingEngine engine)
            : this(Empty, source, destination, sourceType, destinationType, typeMap)
        {
            this.InstanceCache = new Dictionary<ResolutionContext, object>();
            this.Options = options;
            this.Engine = engine;
        }

        private ResolutionContext(ResolutionContext context, object sourceValue, Type sourceType) : this(context, sourceValue, context.DestinationValue, sourceType)
        {
        }

        private ResolutionContext(ResolutionContext context, TypeMap memberTypeMap, object sourceValue,
            object destinationValue, Type sourceType, Type destinationType) 
            : this(context, sourceValue, destinationValue, sourceType, destinationType, memberTypeMap)
        {
        }

        private ResolutionContext(ResolutionContext context, object sourceValue, object destinationValue,
            TypeMap memberTypeMap, PropertyMap propertyMap) : this(context, sourceValue, destinationValue, null, null, memberTypeMap)
        {
            if(memberTypeMap == null)
            {
                throw new ArgumentNullException(nameof(memberTypeMap));
            }
            this.PropertyMap = propertyMap;
        }

        private ResolutionContext(ResolutionContext context, object sourceValue, object destinationValue,
            Type sourceType, PropertyMap propertyMap) : this(context, sourceValue, destinationValue, sourceType, propertyMap.DestinationProperty.MemberType == typeof(object) ? sourceType : propertyMap.DestinationProperty.MemberType)
        {
            this.PropertyMap = propertyMap;
        }

        private ResolutionContext(ResolutionContext context, object sourceValue, TypeMap typeMap, Type sourceType,
            Type destinationType, int arrayIndex) : this(context, sourceValue, null, sourceType, destinationType, typeMap)
        {
            this.ArrayIndex = arrayIndex;
        }

        public string MemberName => this.PropertyMap == null
            ? string.Empty
            : (this.ArrayIndex == null
                ? this.PropertyMap.DestinationProperty.Name
                : this.PropertyMap.DestinationProperty.Name + this.ArrayIndex.Value);

        public bool IsSourceValueNull => Equals(null, this.SourceValue);


        public ResolutionContext CreateValueContext(object sourceValue, Type sourceType)
        {
            return new ResolutionContext(this, sourceValue, sourceType);
        }

        public ResolutionContext CreateTypeContext(TypeMap memberTypeMap, object sourceValue, object destinationValue,
            Type sourceType, Type destinationType)
        {
            return new ResolutionContext(this, memberTypeMap, sourceValue, destinationValue, sourceType, destinationType);
        }

        public ResolutionContext CreatePropertyMapContext(PropertyMap propertyMap)
        {
            return new ResolutionContext(this, this.SourceValue, this.DestinationValue, this.SourceType, propertyMap);
        }

        public ResolutionContext CreateMemberContext(TypeMap memberTypeMap, object memberValue, object destinationValue,
            Type sourceMemberType, PropertyMap propertyMap)
        {
            return memberTypeMap != null
                ? new ResolutionContext(this, memberValue, destinationValue, memberTypeMap, propertyMap)
                : new ResolutionContext(this, memberValue, destinationValue, sourceMemberType, propertyMap);
        }

        public ResolutionContext CreateElementContext(TypeMap elementTypeMap, object item, Type sourceElementType,
            Type destinationElementType, int arrayIndex)
        {
            return new ResolutionContext(this, item, elementTypeMap, sourceElementType, destinationElementType,
                arrayIndex);
        }

        public override string ToString()
        {
            return $"Trying to map {this.SourceType.Name} to {this.DestinationType.Name}.";
        }

        public TypeMap GetContextTypeMap()
        {
            TypeMap typeMap = this.TypeMap;
            ResolutionContext parent = this.Parent;
            while ((typeMap == null) && (parent != null))
            {
                typeMap = parent.TypeMap;
                parent = parent.Parent;
            }
            return typeMap;
        }

        public PropertyMap GetContextPropertyMap()
        {
            PropertyMap propertyMap = this.PropertyMap;
            ResolutionContext parent = this.Parent;
            while ((propertyMap == null) && (parent != null))
            {
                propertyMap = parent.PropertyMap;
                parent = parent.Parent;
            }
            return propertyMap;
        }

        public bool Equals(ResolutionContext other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.TypeMap, this.TypeMap) && Equals(other.SourceType, this.SourceType) &&
                   Equals(other.DestinationType, this.DestinationType) && Equals(other.SourceValue, this.SourceValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ResolutionContext)) return false;
            return this.Equals((ResolutionContext) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (this.TypeMap != null ? this.TypeMap.GetHashCode() : 0);
                result = (result*397) ^ (this.SourceType != null ? this.SourceType.GetHashCode() : 0);
                result = (result*397) ^ (this.DestinationType != null ? this.DestinationType.GetHashCode() : 0);
                result = (result*397) ^ (this.SourceValue != null ? this.SourceValue.GetHashCode() : 0);
                return result;
            }
        }

        public ResolutionContext[] GetContexts()
        {
            return this.GetContextsCore().Reverse().Distinct().ToArray();
        }

        protected IEnumerable<ResolutionContext> GetContextsCore()
        {
            var context = this;
            while(context.Parent != null)
            {
                yield return context;
                context = context.Parent;
            }
            yield return context;
        }

        public static ResolutionContext New<TSource>(TSource sourceValue, IMappingEngine mappingEngine)
        {
            return new ResolutionContext(null, sourceValue, typeof (TSource), null, new MappingOperationOptions(), mappingEngine);
        }

        internal void BeforeMap(object destination)
        {
            if(this.Parent == null)
            {
                this.Options.BeforeMapAction(this.SourceValue, destination);
            }
        }

        internal void AfterMap(object destination)
        {
            if(this.Parent == null)
            {
                this.Options.AfterMapAction(this.SourceValue, destination);
            }
        }
    }
}