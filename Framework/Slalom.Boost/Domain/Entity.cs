using System;
using System.Diagnostics.CodeAnalysis;
using MassTransit;

namespace Slalom.Boost.Domain
{
    /// <summary>
    /// The base class for an <see href="https://www.safaribooksonline.com/library/view/microsoft-net-architecting/9780133986419/ch08.html#ch08lev2sec3">Entity</see>, 
    /// an object that represents a thread of continuity and identity, going through a lifecycle.
    /// </summary>
    /// <seealso href="https://www.safaribooksonline.com/library/view/domain-driven-design-tackling/0321125215/ch06.html">Domain-Driven Design: Tackling Complexity in the Heart of Software: Six. The Life Cycle of a Domain Object</seealso>
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        protected Entity()
            : this(NewId.NextGuid())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        protected Entity(Guid id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        /// <value>The entity identifier.</value>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the entity keys.  This may be combinatino of human and non-human readable keys.
        /// </summary>
        /// <returns>Returns the entity keys.</returns>
        public virtual object GetKeys()
        {
            return this.Id;
        }

        #region Equality Members

        /// <summary>
        /// Determines whether the specified <see cref="Entity" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        protected bool Equals(Entity other)
        {
            return other != null && this.Id.Equals(other.Id);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((Entity)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion
    }
}