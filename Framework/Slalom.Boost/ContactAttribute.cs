using System;

namespace Slalom.Boost
{
    /// <summary>
    /// Specifies the contact person for a class.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ContactAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAttribute"/> class.
        /// </summary>
        /// <param name="email">The contact's email.</param>
        public ContactAttribute(string email)
            : this(null, email)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAttribute"/> class.
        /// </summary>
        /// <param name="name">The contact's name.</param>
        /// <param name="email">The contact's email.</param>
        public ContactAttribute(string name, string email)
        {
            this.Name = name;
            this.Email = email;
        }

        /// <summary>
        /// Gets or sets the contact's email.
        /// </summary>
        /// <value>The owner's email.</value>
        public string Email { get; private set; }

        /// <summary>
        /// Gets or sets the contact's name.
        /// </summary>
        /// <value>The owner's name.</value>
        public string Name { get; private set; }
    }
}