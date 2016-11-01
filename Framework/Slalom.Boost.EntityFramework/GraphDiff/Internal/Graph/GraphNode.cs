using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Slalom.Boost.EntityFramework.GraphDiff.Internal.Graph
{
    internal class GraphNode
    {       
        protected readonly PropertyInfo Accessor;

        protected string IncludeString
        {
            get
            {
                var ownIncludeString = Accessor != null ? Accessor.Name : null;
                return this.Parent != null && this.Parent.IncludeString != null
                        ? this.Parent.IncludeString + "." + ownIncludeString
                        : ownIncludeString;
            }
        }

        public GraphNode Parent { get; set; }
        public Stack<GraphNode> Members { get; private set; }

        public GraphNode()
        {
            this.Members = new Stack<GraphNode>();
        }

        protected GraphNode(GraphNode parent, PropertyInfo accessor)
        {
            Accessor = accessor;
            this.Members = new Stack<GraphNode>();
            this.Parent = parent;
        }

        // overridden by different implementations
        public virtual void Update<T>(IChangeTracker changeTracker, IEntityManager entityManager, T persisted, T updating) where T : class
        {
            changeTracker.UpdateItem(updating, persisted, true);

            // Foreach branch perform recursive update
            foreach (var member in this.Members)
            {
                member.Update(changeTracker, entityManager, persisted, updating);
            }
        }

        public List<string> GetIncludeStrings(IEntityManager entityManager)
        {
            var includeStrings = new List<string>();
            var ownIncludeString = this.IncludeString;
            if (!string.IsNullOrEmpty(ownIncludeString))
            {
                includeStrings.Add(ownIncludeString);
            }

            includeStrings.AddRange(this.GetRequiredNavigationPropertyIncludes(entityManager));

            foreach (var member in this.Members)
            {
                includeStrings.AddRange(member.GetIncludeStrings(entityManager));
            }

            return includeStrings;
        }

        public string GetUniqueKey()
        {
            string key = "";
            if (this.Parent != null && this.Parent.Accessor != null)
            {
                key += this.Parent.Accessor.DeclaringType.FullName + "_" + this.Parent.Accessor.Name;
            }
            else
            {
                key += "NoParent";
            }
            return key + "_" + Accessor.DeclaringType.FullName + "_" + Accessor.Name;
        }

        protected T GetValue<T>(object instance)
        {
            return (T)Accessor.GetValue(instance, null);
        }

        protected void SetValue(object instance, object value)
        {
            Accessor.SetValue(instance, value, null);
        }

        protected virtual IEnumerable<string> GetRequiredNavigationPropertyIncludes(IEntityManager entityManager)
        {
            return new string[0];
        }

        protected List<string> GetMappedNaviationProperties()
        {
            return this.Members.Select(m => m.Accessor.Name).ToList();
        }

        protected static IEnumerable<string> GetRequiredNavigationPropertyIncludes(IEntityManager entityManager, Type entityType, string ownIncludeString)
        {
            return entityManager
                .GetRequiredNavigationPropertiesForType(entityType)
                .Select(navigationProperty => ownIncludeString + "." + navigationProperty.Name);
        }

        protected static void ThrowIfCollectionType(PropertyInfo accessor, string mappingType)
        {
            if (IsCollectionType(accessor.PropertyType))
                throw new ArgumentException(string.Format("Collection '{0}' can not be mapped as {1} entity. Please map it as {1} collection.", accessor.Name, mappingType));
        }

        private static bool IsCollectionType(Type propertyType)
        {
            return propertyType.IsArray || propertyType.GetInterface(typeof (IEnumerable<>).FullName) != null;
        }
    }
}
