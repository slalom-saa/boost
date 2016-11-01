using System;
using System.Collections.Generic;
using Slalom.Boost.Events;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Domain
{
    /// <summary>
    /// Defines an entity.
    /// </summary>
    /// <seealso href="https://www.safaribooksonline.com/library/view/domain-driven-design-tackling/0321125215/ch06.html">Domain-Driven Design: Tackling Complexity in the Heart of Software: Six. The Life Cycle of a Domain Object</seealso>
    [IgnoreBinding]
    public interface IEntity : IHaveIdentity
    {
    }
}