using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using Slalom.Boost.Reflection;

namespace Slalom.Boost.EntityFramework.GraphDiff.Internal
{
    internal class EntityManager : IEntityManager
    {
        private readonly DbContext _context;
        private ObjectContext ObjectContext
        {
            get { return ((IObjectContextAdapter)_context).ObjectContext; }
        }

        public EntityManager(DbContext context)
        {
            _context = context;
        }

        public EntityKey CreateEntityKey(object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            return this.ObjectContext.CreateEntityKey(this.GetEntitySetName(entity.GetType()), entity);
        }

        public bool AreKeysIdentical(object newValue, object dbValue)
        {
            if (newValue == null || dbValue == null)
            {
                return false;
            }

            return this.CreateEntityKey(newValue) == this.CreateEntityKey(dbValue);
        }

        public object CreateEmptyEntityWithKey(object entity)
        {
            var instance = Activator.CreateInstance(entity.GetType(), true);
            this.CopyPrimaryKeyFields(entity, instance);
            return instance;
        }

        public IEnumerable<PropertyInfo> GetPrimaryKeyFieldsFor(Type entityType)
        {
            var metadata = this.ObjectContext.MetadataWorkspace
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .SingleOrDefault(p => p.FullName == entityType.FullName);

            if (metadata == null)
            {
                throw new InvalidOperationException(String.Format("The type {0} is not known to the DbContext.", entityType.FullName));
            }

            return metadata.KeyMembers
                .Select(k => entityType.GetProperty(k.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                .ToList();
        }

        public IEnumerable<NavigationProperty> GetRequiredNavigationPropertiesForType(Type entityType)
        {
            return this.GetNavigationPropertiesForType(ObjectContext.GetObjectType(entityType))
                    .Where(navigationProperty => navigationProperty.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.One);
        }

        public IEnumerable<NavigationProperty> GetNavigationPropertiesForType(Type entityType)
        {
            return this.ObjectContext.MetadataWorkspace
                    .GetItems<EntityType>(DataSpace.OSpace)
                    .Single(p => p.FullName == entityType.FullName)
                    .NavigationProperties;
        }

        private string GetEntitySetName(Type entityType)
        {
            Type type = entityType;
            EntitySetBase set = null;

            while (set == null && type != null)
            {
                set = this.ObjectContext.MetadataWorkspace
                        .GetEntityContainer(this.ObjectContext.DefaultContainerName, DataSpace.CSpace)
                        .EntitySets
                        .FirstOrDefault(item => item.ElementType.Name.Equals(type.Name));

                type = type.BaseType;
            }

            return set != null ? set.Name : null;
        }

        private void CopyPrimaryKeyFields(object from, object to)
        {
            var keyProperties = this.GetPrimaryKeyFieldsFor(from.GetType());
            foreach (var keyProperty in keyProperties)
            {
                if (keyProperty.CanWrite)
                {
                    keyProperty.SetValue(to, keyProperty.GetValue(from, null), null);
                }
                else
                {
                    var backing = keyProperty.GetBackingField();
                    backing.SetValue(to, keyProperty.GetValue(from, null));
                }
            }
        }
    }
}
