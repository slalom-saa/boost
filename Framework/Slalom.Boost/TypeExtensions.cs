using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.CSharp;

namespace Slalom.Boost
{
    /// <summary>
    /// Contains extensions for <see cref="Type"/> classes.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns all custom attributes of the specified type.
        /// </summary>
        /// <typeparam name="T">The attribute type</typeparam>
        /// <param name="type">The instance.</param>
        /// <returns>Returns all custom attributes of the specified type.</returns>
        public static IEnumerable<T> GetAllAttributes<T>(this Type type) where T : Attribute
        {
            var target = new List<T>();
            do
            {
                target.AddRange(type.GetCustomAttributes<T>());
                target.AddRange(type.GetInterfaces().SelectMany(e => e.GetAllAttributes<T>()));
                type = type.BaseType;
            }
            while (type != null);

            return target.AsEnumerable();
        }

        /// <summary>
        /// Safely gets all types when some referenced assemblies may not be available.
        /// </summary>
        /// <param name="assemblies">The instance.</param>
        /// <returns>Returns all types when some referenced assemblies may not be available.</returns>
        public static Type[] SafelyGetTypes<T>(this IEnumerable<_Assembly> assemblies)
        {
            return assemblies.SelectMany(e => e.SafelyGetTypes()).Where(e => (typeof(T).IsAssignableFrom(e))).ToArray();
        }

        /// <summary>
        /// Safely gets all types when some referenced assemblies may not be available.
        /// </summary>
        /// <param name="assemblies">The instance.</param>
        /// <returns>Returns all types when some referenced assemblies may not be available.</returns>
        public static Type[] SafelyGetTypes(this IEnumerable<_Assembly> assemblies)
        {
            return assemblies.SelectMany(e => e.SafelyGetTypes()).ToArray();
        }

        /// <summary>
        /// Safely gets all types when some referenced assemblies may not be available.
        /// </summary>
        /// <param name="assembly">The instance.</param>
        /// <returns>Returns all types when some referenced assemblies may not be available.</returns>
        public static Type[] SafelyGetTypes(this _Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException exception)
            {
                return exception.Types.Where(x => x != null).ToArray();
            }
            catch
            {
                return new Type[0];
            }
        }

        /// <summary>
        /// Returns the friendly name.
        /// </summary>
        /// <param name="type">The instance.</param>
        /// <returns>Returns the friendly name.</returns>
        public static string ToFriendlyName(this Type type)
        {
            using (var provider = new CSharpCodeProvider())
            {
                var typeRef = new CodeTypeReference(type);
                return provider.GetTypeOutput(typeRef);
            }
        }

        /// <summary>
        /// Gets all base and contract types.
        /// </summary>
        /// <param name="type">The instance.</param>
        /// <returns>Returns all base and contract types.</returns>
        public static IEnumerable<Type> GetBaseAndContractTypes(this Type type)
        {
            return type.GetBaseTypes().Concat(type.GetInterfaces()).SelectMany(GetTypeAndGeneric).Where(t => t != type && t != typeof(object));
        }

        /// <summary>
        /// Gets all base types in the hierarchy.
        /// </summary>
        /// <param name="type">The instance.</param>
        /// <returns>Returns all base types in the hierarchy.</returns>
        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            var currentType = type;
            while (currentType != null)
            {
                yield return currentType;
                currentType = currentType.BaseType;
            }
        }

        private static IEnumerable<Type> GetTypeAndGeneric(Type type)
        {
            yield return type;
            if (type.IsGenericType && !type.ContainsGenericParameters)
            {
                yield return type.GetGenericTypeDefinition();
            }
        }
    }
}