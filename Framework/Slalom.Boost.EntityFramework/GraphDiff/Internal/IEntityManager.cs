using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Reflection;

namespace Slalom.Boost.EntityFramework.GraphDiff.Internal
{
    /// <summary>Entity creation, type & key management</summary>
    internal interface IEntityManager
    {
        /// <summary>Creates the unique entity key for an entity</summary>
        EntityKey CreateEntityKey(object entity);

        /// <summary>Creates an empty object of the same type and keys matching the entity provided</summary>
        object CreateEmptyEntityWithKey(object entity);

        /// <summary>Returns true if the keys of entity1 and entity2 match.</summary>
        bool AreKeysIdentical(object entity1, object entity2);

        /// <summary>Returns the primary key fields for a given  entity type</summary>
        IEnumerable<PropertyInfo> GetPrimaryKeyFieldsFor(Type entityType);

        /// <summary>Retrieves the required navigation properties for the given type</summary>
        IEnumerable<NavigationProperty> GetRequiredNavigationPropertiesForType(Type entityType);

        /// <summary>Retrieves the navigation properties for the given type</summary>
        IEnumerable<NavigationProperty> GetNavigationPropertiesForType(Type entityType);
    }
}