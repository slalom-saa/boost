using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MongoDB.Bson.Serialization;
using Slalom.Boost.Aspects;

namespace Slalom.Boost.MongoDB
{
    /// <summary>
    /// Contains shared MongoDB extensions.
    /// </summary>
    public static class MongoExtensions
    {
        /// <summary>
        /// Dynamically builds a map for the specified type.
        /// </summary>
        /// <param name="target">The target.</param>
        public static void BuildMap(Type target)
        {
            if (!BsonClassMap.IsClassMapRegistered(target))
            {
                var map = new BsonClassMap(target);

                var properties = target.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var propertyNames = properties.Select(x => x.Name);

                foreach (var info in properties.Where(e => e.DeclaringType == target))
                {
                    if (!info.GetCustomAttributes<SecurePropertyAttribute>().Any())
                    {
                        map.MapProperty(info.Name);
                    }
                }

                var fields = target.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var info in fields.Where(e => e.DeclaringType == target))
                {
                    if (info.FieldType.IsGenericType && info.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var name = Char.ToUpper(info.Name[1]) + info.Name.Substring(2);
                        if (properties.Any(e => e.Name == name))
                        {
                            map.UnmapProperty(name);
                            map.MapField(info.Name).SetElementName(name);
                        }
                    }
                }

                var constructors = target.GetConstructors();

                foreach (var constructorInfo in constructors)
                {
                    var parameters = constructorInfo.GetParameters();

                    if (parameters.All(e => propertyNames.Contains(char.ToUpper(e.Name[0]) + e.Name.Remove(0, 1))))
                    {
                        var parameterExpressions = parameters.Select(e => Expression.Parameter(e.ParameterType, e.Name)).ToList();

                        var expression = Expression.Lambda(Expression.New(constructorInfo, parameterExpressions),
                            parameterExpressions);

                        map.MapCreator(expression.Compile(), parameters.Select(e => char.ToUpper(e.Name[0]) + e.Name.Remove(0, 1)).ToArray());
                    }
                }

                BsonClassMap.RegisterClassMap(map);
            }
        }
    }
}