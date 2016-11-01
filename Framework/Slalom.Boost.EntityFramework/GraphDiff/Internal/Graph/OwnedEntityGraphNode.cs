using System;
using System.Reflection;

namespace Slalom.Boost.EntityFramework.GraphDiff.Internal.Graph
{
    internal class OwnedEntityGraphNode : GraphNode
    {
        internal OwnedEntityGraphNode(GraphNode parent, PropertyInfo accessor)
                : base(parent, accessor)
        {
            ThrowIfCollectionType(accessor, "owned");
        }

        public override void Update<T>(IChangeTracker changeTracker, IEntityManager entityManager, T persisted, T updating)
        {
            var dbValue = this.GetValue<object>(persisted);
            var newValue = this.GetValue<object>(updating);

            if (dbValue == null && newValue == null)
            {
                return;
            }

            // Merging options
            // 1. No new value, set value to null. entity will be removed if cascade rules set.
            // 2. If new value is same as old value lets update the members
            // 3. Otherwise new value is set and we don't care about old dbValue, so create a new one.
            if (newValue == null)
            {
                this.SetValue(persisted, null);
                return;
            }

            if (dbValue != null && entityManager.AreKeysIdentical(newValue, dbValue))
            {
                changeTracker.UpdateItem(newValue, dbValue, true);
            }
            else
            {
                dbValue = this.CreateNewPersistedEntity(changeTracker, persisted, newValue);
            }

            changeTracker.AttachCyclicNavigationProperty(persisted, newValue, this.GetMappedNaviationProperties());

            foreach (var childMember in this.Members)
            {
                childMember.Update(changeTracker, entityManager, dbValue, newValue);
            }
        }

        private object CreateNewPersistedEntity<T>(IChangeTracker changeTracker, T existing, object newValue) where T : class
        {
            var instance = Activator.CreateInstance(newValue.GetType(), true);
            this.SetValue(existing, instance);
            changeTracker.AddItem(instance);
            changeTracker.UpdateItem(newValue, instance, true);
            return instance;
        }
    }
}
