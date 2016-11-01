using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Slalom.Boost.VisualStudio.RuntimeBinding.Configuration;

namespace Slalom.Boost.VisualStudio.RuntimeBinding
{
    /// <summary>
    /// Contains extensions for <see cref="IContainer"/> classes.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class IContainerExtensions
    {
        /// <summary>
        /// Automatically configures the container using type discovery.
        /// </summary>
        /// <typeparam name="T">The type of container</typeparam>
        /// <param name="container">The container to configure.</param>
        /// <param name="filters">Any binding filters to use.</param>
        /// <returns>The type of passed in container.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static T AutoConfigure<T>(this T container, params BindingFilter[] filters) where T : IContainer
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            var configuration = new RuntimeBindingConfigurator(filters);

            configuration.ConfigureContainer(container);

            return container;
        }

        /// <summary>
        /// Automatically configures the container using type discovery.
        /// </summary>
        /// <typeparam name="T">The type of container</typeparam>
        /// <param name="container">The container to configure.</param>
        /// <param name="type">A type in the root assembly to use as a binding filter.  It will take use the first part of the name (everything before the first '.').</param>
        /// <returns>The type of passed in container.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public static T AutoConfigure<T>(this T container, Type type) where T : IContainer
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            var configuration = new RuntimeBindingConfigurator(AssemblyFilter.Include(e => e.FullName.StartsWith(type.Assembly.FullName.Split('.')[0])));

            configuration.ConfigureContainer(container);

            return container;
        }

        /// <summary>
        /// Automatically configures the container using type discovery.
        /// </summary>
        /// <typeparam name="T">The type of container</typeparam>
        /// <param name="container">The container to configure.</param>
        /// <param name="type">A type in the root assembly to use as a binding filter.  It will take use the first part of the name (everything before the first '.').</param>
        /// <param name="filters">Any binding filters to use.</param>
        /// <returns>The type of passed in container.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public static T AutoConfigure<T>(this T container, Type type, params BindingFilter[] filters) where T : IContainer
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            var configuration = new RuntimeBindingConfigurator(filters.Concat(
                new[]
                {
                    AssemblyFilter.Include(e => e.FullName.StartsWith(type.Assembly.FullName.Split('.')[0]))
                }).ToArray());

            configuration.ConfigureContainer(container);

            return container;
        }
    }
}