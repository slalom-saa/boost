using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slalom.Boost
{
    /// <summary>
    /// Defines a contract for an object that has an identity.
    /// </summary>
    public interface IHaveIdentity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        Guid Id { get; }
    }
}
