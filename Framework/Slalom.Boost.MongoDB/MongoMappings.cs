using System;
using System.Linq;
using MongoDB.Bson.Serialization;
using Slalom.Boost.Domain;
using Slalom.Boost.Events;
using Slalom.Boost.RuntimeBinding.Configuration;

namespace Slalom.Boost.MongoDB
{
    /// <summary>
    /// Contains the standard MongoDB mappings.
    /// </summary>
    public static class MongoMappings
    {
        private static bool initialized;


        /// <summary>
        /// Ensures that the mappings are initialized.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static void EnsureInitialized(object instance)
        {
            if (!initialized)
            {
                initialized = true;

                CreateKnownMaps();

                CreateDynamicMaps(instance);
            }
        }

        private static void CreateDynamicMaps(object instance)
        {
            var filter = AssemblyFilter.Include(a => a.FullName.StartsWith(instance.GetType().Assembly.FullName.Split('.')[0]));

            var codeBase = new CodeBaseAssemblyLocator().Locate(filter);
            var domain = new AppDomainAssemblyLocator().Locate(filter);
            var all = codeBase.Union(domain).ToList();

            all.SafelyGetTypes<Event>().ToList().ForEach(MongoExtensions.BuildMap);

            all.SafelyGetTypes<Entity>().ToList().ForEach(MongoExtensions.BuildMap);
        }

        private static void CreateKnownMaps()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Entity)))
            {
                BsonClassMap.RegisterClassMap<Entity>(x =>
                {
                    x.AutoMap();
                    x.SetIsRootClass(true);
                    x.MapIdField(e => e.Id);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Event)))
            {
                BsonClassMap.RegisterClassMap<Event>(x =>
                {
                    x.AutoMap();
                    x.SetIsRootClass(true);
                    x.MapProperty(e => e.EventName);
                    x.MapProperty(e => e.TimeStamp);
                });
            }
        }
    }
}