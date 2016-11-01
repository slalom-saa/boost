using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Slalom.Boost.EntityFramework.GraphDiff.Internal.Graph
{
    internal class CollectionGraphNode : GraphNode
    {
        private readonly bool _isOwned;

        internal CollectionGraphNode(GraphNode parent, PropertyInfo accessor, bool isOwned)
            : base(parent, accessor)
        {
            _isOwned = isOwned;
        }

        public override void Update<T>(IChangeTracker changeTracker, IEntityManager entityManager, T existing, T entity)
        {
            var innerElementType = this.GetCollectionElementType();
            var updateValues = this.GetValue<IEnumerable>(entity) ?? new List<object>();
            var dbCollection = this.GetValue<IEnumerable>(existing) ?? this.CreateMissingCollection(existing, innerElementType);

            var dbHash = dbCollection.Cast<object>().ToDictionary(item => entityManager.CreateEntityKey(item));

            // Iterate through the elements from the updated graph and try to match them against the db graph
            var updateList = updateValues.OfType<object>().ToList();
            for (int i = 0; i < updateList.Count; i++)
            {
                var updateItem = updateList[i];
                var key = entityManager.CreateEntityKey(updateItem);

                // try to find item with same key in db collection
                object dbItem;
                if (dbHash.TryGetValue(key, out dbItem))
                {
                    this.UpdateElement(changeTracker, entityManager, existing, updateItem, dbItem);
                    dbHash.Remove(key);
                }
                else
                {
                    updateList[i] = this.AddElement(changeTracker, entityManager, existing, updateItem, dbCollection);
                }
            }

            // remove obsolete items
            foreach (var dbItem in dbHash.Values)
            {
                this.RemoveElement(changeTracker, dbItem, dbCollection);
            }
        }

        private object AddElement<T>(IChangeTracker changeTracker, IEntityManager entityManager, T existing, object updateItem, object dbCollection)
        {
            if (!_isOwned)
            {
                updateItem = changeTracker.AttachAndReloadAssociatedEntity(updateItem);
            }
            else if (changeTracker.GetItemState(updateItem) == EntityState.Detached)
            {
                var instance = entityManager.CreateEmptyEntityWithKey(updateItem);

                changeTracker.AddItem(instance);
                changeTracker.UpdateItem(updateItem, instance);

                foreach (var childMember in this.Members)
                {
                    childMember.Update(changeTracker, entityManager, instance, updateItem);
                }

                updateItem = instance;
            }

            dbCollection.GetType().GetMethod("Add").Invoke(dbCollection, new[] {updateItem});

            if (_isOwned)
            {
                changeTracker.AttachCyclicNavigationProperty(existing, updateItem, this.GetMappedNaviationProperties());
            }

            return updateItem;
        }

        private void UpdateElement<T>(IChangeTracker changeTracker, IEntityManager entityManager, T existing, object updateItem, object dbItem)
        {
            if (!_isOwned)
            {
                return;
            }

            changeTracker.UpdateItem(updateItem, dbItem, true);
            changeTracker.AttachCyclicNavigationProperty(existing, updateItem, this.GetMappedNaviationProperties());

            foreach (var childMember in this.Members)
            {
                childMember.Update(changeTracker, entityManager, dbItem, updateItem);
            }
        }

        private void RemoveElement(IChangeTracker changeTracker, object dbItem, object dbCollection)
        {
            dbCollection.GetType().GetMethod("Remove").Invoke(dbCollection, new[] { dbItem });
            changeTracker.AttachRequiredNavigationProperties(dbItem, dbItem);

            if (_isOwned)
            {
                changeTracker.RemoveItem(dbItem);
            }
        }

        private IEnumerable CreateMissingCollection(object existing, Type elementType)
        {
            var collectionType = !Accessor.PropertyType.IsInterface ? Accessor.PropertyType : typeof(List<>).MakeGenericType(elementType);
            var collection = (IEnumerable)Activator.CreateInstance(collectionType);
            this.SetValue(existing, collection);
            return collection;
        }

        protected override IEnumerable<string> GetRequiredNavigationPropertyIncludes(IEntityManager entityManager)
        {
            if (_isOwned)
            {
                return base.GetRequiredNavigationPropertyIncludes(entityManager);
            }

            return Accessor != null
                    ? GetRequiredNavigationPropertyIncludes(entityManager, this.GetCollectionElementType(), this.IncludeString)
                    : new string[0];
        }

        private Type GetCollectionElementType()
        {
            if (Accessor.PropertyType.IsArray)
            {
                return Accessor.PropertyType.GetElementType();
            }

            if (Accessor.PropertyType.IsGenericType)
            {
                return Accessor.PropertyType.GetGenericArguments()[0];
            }

            throw new InvalidOperationException("GraphDiff requires the collection to be either IEnumerable<T> or T[]");
        }
    }
}
