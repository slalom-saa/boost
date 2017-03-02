using System;
using System.Linq;
using Slalom.Boost.AutoMapper;
using Slalom.Boost.AutoMapper.QueryableExtensions;
using Slalom.Boost.Domain;

namespace Slalom.Boost.Aspects.Default
{
    /// <summary>
    /// Provides a default <see cref="IMapper"/> implementation.
    /// </summary>
    /// <seealso cref="IMapper" />
    public class AutoMapperMapper : IMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperMapper"/> class.
        /// </summary>
        public AutoMapperMapper()
        {
        }

        static AutoMapperMapper()
        {
            Mapper.Initialize(e => e.CreateMissingTypeMaps = true);
        }

        /// <summary>
        /// Maps the specified instance to the specified type.
        /// </summary>
        /// <typeparam name="TOut">The type to map to.</typeparam>
        /// <param name="instance">The instance to map.</param>
        /// <returns>
        /// Returns the mapped type.
        /// </returns>
        public TOut Map<TOut>(object instance)
        {
            return Mapper.Map<TOut>(instance);
        }

        /// <summary>
        /// Projects the specified <see cref="IQueryable" /> instance to the specified type.
        /// </summary>
        /// <typeparam name="TOut">The type of the project to.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// A query of the specified type.
        /// </returns>
        public IQueryable<TOut> ProjectTo<TOut>(IQueryable<object> instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return instance.ProjectTo<TOut>();
        }

        /// <summary>
        /// Maps the specified source to the destination type.
        /// </summary>
        /// <param name="source">The source instance.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>
        /// Returns the mapped object.
        /// </returns>
        public object Map(object source, Type destinationType)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (destinationType == null)
            {
                throw new ArgumentNullException(nameof(destinationType));
            }
            return Mapper.Map(source, source.GetType(), destinationType);
        }
    }
}