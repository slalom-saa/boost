﻿using System;
using System.Linq.Expressions;
using Slalom.Boost.EntityFramework.GraphDiff.Internal.Caching;
using Slalom.Boost.EntityFramework.GraphDiff.Internal.Graph;
using Slalom.Boost.EntityFramework.GraphDiff.Internal.GraphBuilders;

namespace Slalom.Boost.EntityFramework.GraphDiff.Internal
{
    internal class AggregateRegister : IAggregateRegister
    {
        private readonly ICacheProvider _cache;
        private readonly IAttributeGraphBuilder _attributeGraphBuilder;

        public AggregateRegister(ICacheProvider cache)
        {
            _cache = cache;
            _attributeGraphBuilder = new AttributeGraphBuilder();
        }

        public void ClearAll()
        {
            _cache.Clear(typeof(AggregateRegister).FullName);
        }

        public void Register<T>(GraphNode rootNode, string scheme = null)
        {
            _cache.Insert(typeof(AggregateRegister).FullName, GenerateCacheKey<T>(scheme), rootNode);
        }

        public GraphNode GetEntityGraph<T>()
        {
            return _cache.GetOrAdd(typeof (AggregateRegister).FullName, GenerateCacheKey<T>(),
                                   () => _attributeGraphBuilder.CanBuild<T>() ? _attributeGraphBuilder.BuildGraph<T>() : new GraphNode());
        }

        public GraphNode GetEntityGraph<T>(string scheme)
        {
            GraphNode node;
            if (_cache.TryGet(typeof(AggregateRegister).FullName, GenerateCacheKey<T>(scheme), out node))
            {
                return node;
            }

            throw new ArgumentOutOfRangeException("Could not find a mapping scheme with name: '" + scheme + "'");
        }

        public GraphNode GetEntityGraph<T>(Expression<Func<IUpdateConfiguration<T>, object>> expression)
        {
            var key = typeof(T).FullName + "_" + expression;
            return _cache.GetOrAdd(typeof(AggregateRegister).FullName, key, () => new ConfigurationGraphBuilder().BuildGraph(expression));
        }

        private static string GenerateCacheKey<T>(string scheme = null)
        {
            var key = typeof(T).FullName;
            if (!String.IsNullOrEmpty(scheme))
            {
                key += scheme;
            }
            return key;
        }      
    }
}
