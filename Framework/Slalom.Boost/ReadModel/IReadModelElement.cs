using System;

namespace Slalom.Boost.ReadModel
{
    /// <summary>
    /// Marker and constraint for a <see href="https://www.safaribooksonline.com/library/view/microsoft-net-architecting/9780133986419/ch11.html">Read Model</see> element.
    /// </summary>
    /// <seealso href="https://www.safaribooksonline.com/library/view/microsoft-net-architecting/9780133986419/ch11.html">Microsoft .NET: Architecting Applications for the Enterprise, Second Edition: Chapter 11. Implementing CQRS</seealso>
    public interface IReadModelElement : IHaveIdentity
    {
    }
}