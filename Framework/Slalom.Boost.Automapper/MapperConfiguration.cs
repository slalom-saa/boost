using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Slalom.Boost.AutoMapper.Internal;
using Slalom.Boost.AutoMapper.Mappers;
using Slalom.Boost.AutoMapper.QueryableExtensions;
using Slalom.Boost.AutoMapper.QueryableExtensions.Impl;

namespace Slalom.Boost.AutoMapper
{
    public class MapperConfiguration : IConfigurationProvider, IMapperConfiguration
    {
        private readonly ITypeMapFactory _typeMapFactory;
        private readonly IEnumerable<IObjectMapper> _mappers;
        private readonly IEnumerable<ITypeMapObjectMapper> _typeMapObjectMappers;
        private readonly Profile _defaultProfile;

        private readonly ConcurrentDictionary<TypePair, TypeMap> _userDefinedTypeMaps =
            new ConcurrentDictionary<TypePair, TypeMap>();

        private readonly ConcurrentDictionary<TypePair, TypeMap> _typeMapPlanCache =
            new ConcurrentDictionary<TypePair, TypeMap>();

        private readonly ConcurrentDictionary<TypePair, CreateTypeMapExpression> _typeMapExpressionCache =
            new ConcurrentDictionary<TypePair, CreateTypeMapExpression>();

        private readonly ConcurrentDictionary<string, Profile> _profiles =
            new ConcurrentDictionary<string, Profile>();

        private Func<Type, object> _serviceCtor = ObjectCreator.CreateObject;


        public MapperConfiguration(Action<IMapperConfiguration> configure) : this(configure, MapperRegistry.Mappers, TypeMapObjectMapperRegistry.Mappers)
        {
        }

        public MapperConfiguration(Action<IMapperConfiguration> configure, IEnumerable<IObjectMapper> mappers, IEnumerable<ITypeMapObjectMapper> typeMapObjectMappers) 
        {
            _typeMapFactory = new TypeMapFactory();
            _mappers = mappers;
            _typeMapObjectMappers = typeMapObjectMappers;
            _defaultProfile = this.CreateProfile(this.ProfileName);

            configure(this);

            this.Seal();

            this.ExpressionBuilder = new ExpressionBuilder(this);
        }

        public string ProfileName => "";

        #region IConfiguration Members

        void IConfiguration.ForAllMaps(string profileName, Action<TypeMap, IMappingExpression> configuration)
        {
            foreach (var typeMap in _userDefinedTypeMaps.Select(kv => kv.Value).Where(tm => tm.Profile == profileName))
            {
                configuration(typeMap, this.CreateMappingExpression(typeMap));
            }
        }

        IProfileExpression IConfiguration.CreateProfile(string profileName) => this.CreateProfile(profileName);

        void IConfiguration.CreateProfile(string profileName, Action<IProfileExpression> profileConfiguration)
        {
            var profile = this.CreateProfile(profileName);

            profileConfiguration(profile);
        }

        void IConfiguration.AddProfile(Profile profile)
        {
            _profiles.AddOrUpdate(profile.ProfileName, profile, (s, configuration) => profile);

            profile.Initialize(this);
        }

        void IConfiguration.AddProfile<TProfile>() => ((IConfiguration)this).AddProfile(new TProfile());

        void IConfiguration.ConstructServicesUsing(Func<Type, object> constructor) => _serviceCtor = constructor;

        Func<PropertyInfo, bool> IProfileExpression.ShouldMapProperty
        {
            get { return _defaultProfile.ShouldMapProperty; }
            set { _defaultProfile.ShouldMapProperty = value; }
        }

        Func<FieldInfo, bool> IProfileExpression.ShouldMapField
        {
            get { return _defaultProfile.ShouldMapField; }
            set { _defaultProfile.ShouldMapField = value; }
        }

        bool IProfileExpression.CreateMissingTypeMaps
        {
            get { return _defaultProfile.CreateMissingTypeMaps; }
            set { _defaultProfile.CreateMissingTypeMaps = value; }
        }

        void IProfileExpression.IncludeSourceExtensionMethods(Assembly assembly)
        {
            _defaultProfile.IncludeSourceExtensionMethods(assembly);
        }

        INamingConvention IProfileExpression.SourceMemberNamingConvention
        {
            get { return _defaultProfile.SourceMemberNamingConvention; }
            set { _defaultProfile.SourceMemberNamingConvention = value; }
        }

        INamingConvention IProfileExpression.DestinationMemberNamingConvention
        {
            get { return _defaultProfile.DestinationMemberNamingConvention; }
            set { _defaultProfile.DestinationMemberNamingConvention = value; }
        }

        bool IProfileExpression.AllowNullDestinationValues
        {
            get { return this.AllowNullDestinationValues; }
            set { this.AllowNullDestinationValues = value; }
        }

        bool IProfileExpression.AllowNullCollections
        {
            get { return this.AllowNullCollections; }
            set { this.AllowNullCollections = value; }
        }

        void IProfileExpression.ForAllMaps(Action<TypeMap, IMappingExpression> configuration) => _defaultProfile.ForAllMaps(configuration);

        IMemberConfiguration IProfileExpression.AddMemberConfiguration() => _defaultProfile.AddMemberConfiguration();

        IConditionalObjectMapper IProfileExpression.AddConditionalObjectMapper() => _defaultProfile.AddConditionalObjectMapper();

        void IProfileExpression.DisableConstructorMapping() => _defaultProfile.DisableConstructorMapping();

        IMappingExpression<TSource, TDestination> IProfileExpression.CreateMap<TSource, TDestination>() 
            => _defaultProfile.CreateMap<TSource, TDestination>();

        IMappingExpression<TSource, TDestination> IProfileExpression.CreateMap<TSource, TDestination>(MemberList memberList)
            => _defaultProfile.CreateMap<TSource, TDestination>(memberList);

        IMappingExpression<TSource, TDestination> IConfiguration.CreateMap<TSource, TDestination>(string profileName)
            => ((IConfiguration)this).CreateMap<TSource, TDestination>(profileName, MemberList.Destination);

        IMappingExpression<TSource, TDestination> IConfiguration.CreateMap<TSource, TDestination>(string profileName,
            MemberList memberList)
        {
            TypeMap typeMap = this.CreateTypeMap(new TypePair(typeof(TSource), typeof(TDestination)), profileName, memberList);

            return this.CreateMappingExpression<TSource, TDestination>(typeMap);
        }

        IMappingExpression IProfileExpression.CreateMap(Type sourceType, Type destinationType)
            => _defaultProfile.CreateMap(sourceType, destinationType, MemberList.Destination);

        IMappingExpression IProfileExpression.CreateMap(Type sourceType, Type destinationType, MemberList memberList)
            => _defaultProfile.CreateMap(sourceType, destinationType, memberList);

        IMappingExpression IConfiguration.CreateMap(Type sourceType, Type destinationType, MemberList memberList, string profileName)
        {
            var typePair = new TypePair(sourceType, destinationType);

            if (sourceType.IsGenericTypeDefinition() && destinationType.IsGenericTypeDefinition())
            {
                var expression = _typeMapExpressionCache.GetOrAdd(typePair, tp => new CreateTypeMapExpression(tp, memberList, profileName));

                return expression;
            }

            var typeMap = this.CreateTypeMap(typePair, profileName, memberList);

            return this.CreateMappingExpression(typeMap);
        }

        void IProfileExpression.ClearPrefixes() => _defaultProfile.ClearPrefixes();

        void IProfileExpression.RecognizeAlias(string original, string alias) => _defaultProfile.RecognizeAlias(original, alias);

        void IProfileExpression.ReplaceMemberName(string original, string newValue) => _defaultProfile.ReplaceMemberName(original, newValue);

        void IProfileExpression.RecognizePrefixes(params string[] prefixes) => _defaultProfile.RecognizePrefixes(prefixes);

        void IProfileExpression.RecognizePostfixes(params string[] postfixes) => _defaultProfile.RecognizePostfixes(postfixes);

        void IProfileExpression.RecognizeDestinationPrefixes(params string[] prefixes) => _defaultProfile.RecognizeDestinationPrefixes(prefixes);

        void IProfileExpression.RecognizeDestinationPostfixes(params string[] postfixes) => _defaultProfile.RecognizeDestinationPostfixes(postfixes);

        void IProfileExpression.AddGlobalIgnore(string startingwith) => _defaultProfile.AddGlobalIgnore(startingwith);

        #endregion

        #region IConfigurationProvider members

        public IExpressionBuilder ExpressionBuilder { get; }

        public Func<Type, object> ServiceCtor => _serviceCtor;

        public bool AllowNullDestinationValues
        {
            get { return _defaultProfile.AllowNullDestinationValues; }
            private set { _defaultProfile.AllowNullDestinationValues = value; }
        }

        public bool AllowNullCollections
        {
            get { return _defaultProfile.AllowNullCollections; }
            private set { _defaultProfile.AllowNullCollections = value; }
        }

        public TypeMap[] GetAllTypeMaps()
        {
            return _userDefinedTypeMaps.Select(kv => kv.Value).ToArray();
        }

        public TypeMap FindTypeMapFor(Type sourceType, Type destinationType)
        {
            var typePair = new TypePair(sourceType, destinationType);

            return this.FindTypeMapFor(typePair);
        }

        public TypeMap FindTypeMapFor<TSource, TDestination>()
        {
            var typePair = new TypePair(typeof(TSource), typeof(TDestination));

            return this.FindTypeMapFor(typePair);
        }

        public TypeMap FindTypeMapFor(TypePair typePair)
        {
            TypeMap typeMap;
            _userDefinedTypeMaps.TryGetValue(typePair, out typeMap);
            return typeMap;
        }

        public TypeMap ResolveTypeMap(Type sourceType, Type destinationType)
        {
            var typePair = new TypePair(sourceType, destinationType);

            return this.ResolveTypeMap(typePair);
        }

        public TypeMap ResolveTypeMap(TypePair typePair)
        {

            var typeMap = _typeMapPlanCache.GetOrAdd(typePair,
                _ =>
                    this.GetRelatedTypePairs(_)
                        .Select(
                            tp =>
                                _typeMapPlanCache.GetOrDefault(tp) ??
                                this.FindTypeMapFor(tp) ??
                                (!this.CoveredByObjectMap(typePair)
                                    ? this.FindConventionTypeMapFor(tp) ??
                                      this.FindClosedGenericTypeMapFor(tp)
                                    : null))
                        .FirstOrDefault(tm => tm != null));

            return typeMap;
        }

        public TypeMap ResolveTypeMap(object source, object destination, Type sourceType, Type destinationType)
        {
            return this.ResolveTypeMap(source?.GetType() ?? sourceType, destination?.GetType() ?? destinationType);
        }

        public TypeMap ResolveTypeMap(ResolutionResult resolutionResult, Type destinationType)
        {
            return this.ResolveTypeMap(resolutionResult.Type, destinationType) ?? this.ResolveTypeMap(resolutionResult.MemberType, destinationType);
        }

        public IProfileConfiguration GetProfileConfiguration(string profileName)
        {
            return this.GetProfile(profileName);
        }

        public void AssertConfigurationIsValid(TypeMap typeMap)
        {
            this.AssertConfigurationIsValid(Enumerable.Repeat(typeMap, 1));
        }

        public void AssertConfigurationIsValid(string profileName)
        {
            this.AssertConfigurationIsValid(_userDefinedTypeMaps.Select(kv => kv.Value).Where(typeMap => typeMap.Profile == profileName));
        }

        public void AssertConfigurationIsValid<TProfile>()
            where TProfile : Profile, new()
        {
            this.AssertConfigurationIsValid(new TProfile().ProfileName);
        }

        public void AssertConfigurationIsValid()
        {
            this.AssertConfigurationIsValid(_userDefinedTypeMaps.Select(kv => kv.Value));
        }

        public IEnumerable<IObjectMapper> GetMappers()
        {
            return _mappers;
        }

        public IEnumerable<ITypeMapObjectMapper> GetTypeMapMappers() => _typeMapObjectMappers;

        #endregion

        internal void Seal()
        {
            var derivedMaps = new List<Tuple<TypePair, TypeMap>>();
            var redirectedTypes = new List<Tuple<TypePair, TypePair>>();
            foreach (var typeMap in _userDefinedTypeMaps.Select(kv => kv.Value))
            {
                typeMap.Seal();

                _typeMapPlanCache.AddOrUpdate(typeMap.Types, typeMap, (_, _2) => typeMap);
                if (typeMap.DestinationTypeOverride != null)
                {
                    redirectedTypes.Add(Tuple.Create(typeMap.Types, new TypePair(typeMap.SourceType, typeMap.DestinationTypeOverride)));
                }
                derivedMaps.AddRange(this.GetDerivedTypeMaps(typeMap).Select(derivedMap => Tuple.Create(new TypePair(derivedMap.SourceType, typeMap.DestinationType), derivedMap)));
            }
            foreach (var redirectedType in redirectedTypes)
            {
                var derivedMap = this.FindTypeMapFor(redirectedType.Item2);
                if (derivedMap != null)
                {
                    _typeMapPlanCache.AddOrUpdate(redirectedType.Item1, derivedMap, (_, _2) => derivedMap);
                }
            }
            foreach (var derivedMap in derivedMaps)
            {
                _typeMapPlanCache.GetOrAdd(derivedMap.Item1, _ => derivedMap.Item2);
            }
        }


        public IMapper CreateMapper() => new Mapper(this);

        public IMapper CreateMapper(Func<Type, object> serviceCtor) => new Mapper(this, serviceCtor);

        private IEnumerable<TypeMap> GetDerivedTypeMaps(TypeMap typeMap)
        {
            foreach (var derivedMap in typeMap.IncludedDerivedTypes.Select(this.FindTypeMapFor))
            {
                if(derivedMap == null)
                {
                    throw QueryMapperHelper.MissingMapException(typeMap.SourceType, typeMap.DestinationType);
                }
                yield return derivedMap;
                foreach(var derivedTypeMap in this.GetDerivedTypeMaps(derivedMap))
                {
                    yield return derivedTypeMap;
                }
            }
        }

        private Profile CreateProfile(string profileName)
        {
            var profileExpression = new NamedProfile(profileName);

            profileExpression.Initialize(this);

            _profiles.AddOrUpdate(profileExpression.ProfileName, profileExpression,
                (s, configuration) => profileExpression);

            return profileExpression;
        }

        private TypeMap CreateTypeMap(TypePair types, string profileName)
        {
		    return this.CreateTypeMap(types, profileName, MemberList.Destination);
        }

        private TypeMap CreateTypeMap(TypePair types, string profileName, MemberList memberList)
        {
            var typeMap = _userDefinedTypeMaps.GetOrAdd(types, tp =>
            {
                var profileConfiguration = this.GetProfile(profileName);

                var tm = _typeMapFactory.CreateTypeMap(types.SourceType, types.DestinationType, profileConfiguration, memberList);

                tm.Profile = profileName;
                tm.IgnorePropertiesStartingWith = profileConfiguration.GlobalIgnores;

                this.IncludeBaseMappings(types, tm);

                // keep the cache in sync
                TypeMap _;
                _typeMapPlanCache.TryRemove(tp, out _);

                return tm;
            });

            return typeMap;
        }

        private void IncludeBaseMappings(TypePair types, TypeMap typeMap)
        {
            foreach(var inheritedTypeMap in _userDefinedTypeMaps.Select(kv => kv.Value).Where(t => t.TypeHasBeenIncluded(types)))
            {
                typeMap.ApplyInheritedMap(inheritedTypeMap);
                this.IncludeBaseMappings(inheritedTypeMap.Types, typeMap);
            }
        }


        private bool CoveredByObjectMap(TypePair typePair)
        {
            return this.GetMappers().FirstOrDefault(m => m.IsMatch(typePair)) != null;
        }

        private TypeMap FindConventionTypeMapFor(TypePair typePair)
        {
            var matchingTypeMapConfiguration = _profiles.Select(kv => kv.Value).SelectMany(p => p.TypeConfigurations).FirstOrDefault(tc => tc.IsMatch(typePair));
            return matchingTypeMapConfiguration != null ? this.CreateTypeMap(typePair, matchingTypeMapConfiguration.ProfileName) : null;
        }

        private TypeMap FindClosedGenericTypeMapFor(TypePair typePair)
        {
            if(!this.HasOpenGenericTypeMapDefined(typePair))
                return null;
            var closedGenericTypePair = new TypePair(typePair.SourceType, typePair.DestinationType);
            var sourceGenericDefinition = typePair.SourceType.GetGenericTypeDefinition();
            var destGenericDefinition = typePair.DestinationType.GetGenericTypeDefinition();

            var genericTypePair = new TypePair(sourceGenericDefinition, destGenericDefinition);
            CreateTypeMapExpression genericTypeMapExpression;

            if (!_typeMapExpressionCache.TryGetValue(genericTypePair, out genericTypeMapExpression))
            {
                throw new AutoMapperMappingException("Missing type map configuration or unsupported mapping.");
            }

            var typeMap = this.CreateTypeMap(closedGenericTypePair,
                genericTypeMapExpression.ProfileName,
                genericTypeMapExpression.MemberList);

            var mappingExpression = this.CreateMappingExpression(typeMap);

            genericTypeMapExpression.Accept(mappingExpression);

            return typeMap;
        }

        private bool HasOpenGenericTypeMapDefined(TypePair typePair)
        {
            var isGeneric = typePair.SourceType.IsGenericType()
                            && typePair.DestinationType.IsGenericType()
                            && (typePair.SourceType.GetGenericTypeDefinition() != null)
                            && (typePair.DestinationType.GetGenericTypeDefinition() != null);
            if (!isGeneric)
                return false;
            var sourceGenericDefinition = typePair.SourceType.GetGenericTypeDefinition();
            var destGenericDefinition = typePair.DestinationType.GetGenericTypeDefinition();

            var genericTypePair = new TypePair(sourceGenericDefinition, destGenericDefinition);

            return _typeMapExpressionCache.ContainsKey(genericTypePair);
        }

        private IEnumerable<TypePair> GetRelatedTypePairs(TypePair root)
        {
            var subTypePairs =
                from destinationType in this.GetAllTypes(root.DestinationType)
                from sourceType in this.GetAllTypes(root.SourceType)
                select new TypePair(sourceType, destinationType);
            return subTypePairs;
        }

        private IEnumerable<Type> GetAllTypes(Type type)
        {
            yield return type;

            Type baseType = type.BaseType();
            while (baseType != null)
            {
                yield return baseType;
                baseType = baseType.BaseType();
            }

            foreach (var interfaceType in type.GetTypeInfo().ImplementedInterfaces)
            {
                yield return interfaceType;
            }
        }

        private IMappingExpression<TSource, TDestination> CreateMappingExpression<TSource, TDestination>(TypeMap typeMap)
        {
            var profileExpression = this.GetProfile(typeMap.Profile);
            var mappingExp = new MappingExpression<TSource, TDestination>(typeMap, _serviceCtor, profileExpression);
            var type = (typeMap.ConfiguredMemberList == MemberList.Destination) ? typeof(TDestination) : typeof(TSource);
            return this.Ignore(mappingExp, type);
        }

        private IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(IMappingExpression<TSource, TDestination> mappingExp, Type destinationType)
        {
            var destInfo = new TypeDetails(destinationType, ((IProfileExpression)this).ShouldMapProperty, ((IProfileExpression)this).ShouldMapField);
            foreach (var destProperty in destInfo.PublicWriteAccessors)
            {
                var attrs = destProperty.GetCustomAttributes(true);
                if (attrs.Any(x => x is IgnoreMapAttribute))
                {
                    mappingExp = mappingExp.ForMember(destProperty.Name, y => y.Ignore());
                }
                if (_defaultProfile.GlobalIgnores.Contains(destProperty.Name))
                {
                    mappingExp = mappingExp.ForMember(destProperty.Name, y => y.Ignore());
                }
            }
            return mappingExp;
        }

        private IMappingExpression CreateMappingExpression(TypeMap typeMap)
        {
            var profileExpression = this.GetProfile(typeMap.Profile);
            var mappingExp = new MappingExpression(typeMap, _serviceCtor, profileExpression);
            return (IMappingExpression)this.Ignore(mappingExp, typeMap.DestinationType);
        }

        private void AssertConfigurationIsValid(IEnumerable<TypeMap> typeMaps)
        {
            this.Seal();
            var maps = typeMaps as TypeMap[] ?? typeMaps.ToArray();
            var badTypeMaps =
                (from typeMap in maps
                 where typeMap.ShouldCheckForValid()
                    let unmappedPropertyNames = typeMap.GetUnmappedPropertyNames()
                    where unmappedPropertyNames.Length > 0
                    select new AutoMapperConfigurationException.TypeMapConfigErrors(typeMap, unmappedPropertyNames)
                    ).ToArray();

            if (badTypeMaps.Any())
            {
                throw new AutoMapperConfigurationException(badTypeMaps);
            }

            var typeMapsChecked = new List<TypeMap>();
            var configExceptions = new List<Exception>();
            var engine = new MappingEngine(this, this.CreateMapper());

            foreach (var typeMap in maps)
            {
                try
                {
                    this.DryRunTypeMap(typeMapsChecked,
                        new ResolutionContext(typeMap, null, typeMap.SourceType, typeMap.DestinationType,
                            new MappingOperationOptions(), engine));
                }
                catch (Exception e)
                {
                    configExceptions.Add(e);
                }
            }

            if (configExceptions.Count > 1)
            {
                throw new AggregateException(configExceptions);
            }
            if (configExceptions.Count > 0)
            {
                throw configExceptions[0];
            }
        }

        private void DryRunTypeMap(ICollection<TypeMap> typeMapsChecked, ResolutionContext context)
        {
            var typeMap = context.TypeMap;
            if (typeMap != null)
            {
                typeMapsChecked.Add(typeMap);
                this.CheckPropertyMaps(typeMapsChecked, context);
            }
            else
            {
                var mapperToUse = this.GetMappers().FirstOrDefault(mapper => mapper.IsMatch(context.Types));
                if (mapperToUse == null && context.SourceType.IsNullableType())
                {
                    var nullableTypes = new TypePair(Nullable.GetUnderlyingType(context.SourceType),
                        context.DestinationType);
                    mapperToUse = this.GetMappers().FirstOrDefault(mapper => mapper.IsMatch(nullableTypes));
                }
                if (mapperToUse == null)
                {
                    throw new AutoMapperConfigurationException(context);
                }
                if (mapperToUse is ArrayMapper || mapperToUse is EnumerableMapper || mapperToUse is CollectionMapper)
                {
                    this.CheckElementMaps(typeMapsChecked, context);
                }
            }
        }

        private void CheckElementMaps(ICollection<TypeMap> typeMapsChecked, ResolutionContext context)
        {
            Type sourceElementType = TypeHelper.GetElementType(context.SourceType);
            Type destElementType = TypeHelper.GetElementType(context.DestinationType);
            TypeMap itemTypeMap = ((IConfigurationProvider)this).ResolveTypeMap(sourceElementType, destElementType);

            if(typeMapsChecked.Any(typeMap => Equals(typeMap, itemTypeMap)))
                return;

            var memberContext = context.CreateElementContext(itemTypeMap, null, sourceElementType, destElementType,
                0);

            this.DryRunTypeMap(typeMapsChecked, memberContext);
        }

        private void CheckPropertyMaps(ICollection<TypeMap> typeMapsChecked, ResolutionContext context)
        {
            foreach(var propertyMap in context.TypeMap.GetPropertyMaps())
            {
                if(!propertyMap.IsIgnored())
                {
                    var lastResolver =
                        propertyMap.GetSourceValueResolvers().OfType<IMemberResolver>().LastOrDefault();

                    if(lastResolver != null)
                    {
                        var sourceType = lastResolver.MemberType;
                        var destinationType = propertyMap.DestinationProperty.MemberType;
                        var memberTypeMap = ((IConfigurationProvider)this).ResolveTypeMap(sourceType,
                            destinationType);

                        if(typeMapsChecked.Any(typeMap => Equals(typeMap, memberTypeMap)))
                            continue;

                        var memberContext = context.CreateMemberContext(memberTypeMap, null, null, sourceType,
                            propertyMap);

                        this.DryRunTypeMap(typeMapsChecked, memberContext);
                    }
                }
            }
        }

        private Profile GetProfile(string profileName)
        {
            var expr = _profiles.GetOrAdd(profileName, name => new NamedProfile(profileName));

            return expr;
        }

        private class NamedProfile : Profile
        {
            public NamedProfile(string profileName) : base(profileName)
            {
            }

            protected override void Configure()
            {
                // no-op
            }
        }

    }
}
