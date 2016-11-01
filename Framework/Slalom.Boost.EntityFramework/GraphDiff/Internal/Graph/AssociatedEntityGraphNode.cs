﻿using System.Collections.Generic;
using System.Reflection;

namespace Slalom.Boost.EntityFramework.GraphDiff.Internal.Graph
{
    internal class AssociatedEntityGraphNode : GraphNode
    {
        internal AssociatedEntityGraphNode(GraphNode parent, PropertyInfo accessor)
                : base(parent, accessor)
        {
            ThrowIfCollectionType(accessor, "associated");
        }

        public override void Update<T>(IChangeTracker changeTracker, IEntityManager entityManager, T persisted, T updating)
        {
            var dbValue = this.GetValue<object>(persisted);
            var newValue = this.GetValue<object>(updating);

            if (newValue == null)
            {
                this.SetValue(persisted, null);
                return;
            }

            // do nothing if the key is already identical
            if (entityManager.AreKeysIdentical(newValue, dbValue))
            {
                return;
            }

            newValue = changeTracker.AttachAndReloadAssociatedEntity(newValue);
            this.SetValue(persisted, newValue);
        }

        protected override IEnumerable<string> GetRequiredNavigationPropertyIncludes(IEntityManager entityManager)
        {
            return Accessor != null
                    ? GetRequiredNavigationPropertyIncludes(entityManager, Accessor.PropertyType, this.IncludeString)
                    : new string[0];
        }
    }
}
