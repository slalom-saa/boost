﻿using System;

namespace Slalom.Boost.Domain
{
    /// <summary>
    /// Defines an aggregate root.
    /// </summary>
    /// <seealso cref="IEntity" />
    /// <seealso href="https://www.safaribooksonline.com/library/view/domain-driven-design-tackling/0321125215/ch06.html">Domain-Driven Design: Tackling Complexity in the Heart of Software: Six. The Life Cycle of a Domain Object</seealso>
    public interface IAggregateRoot : IEntity
    {
    }
}