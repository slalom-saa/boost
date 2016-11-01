using System;
using System.Linq;
using System.Data.Entity.Migrations;
using Slalom.Boost.EntityFramework;


namespace $safeprojectname$
{
    /// <summary>
    /// Provides a <see href="https://www.safaribooksonline.com/library/view/microsoft-net-architecting/9780133986419/ch10.html">Read Model Façade</see> for the module.
    /// </summary>
    /// <seealso href="https://www.safaribooksonline.com/library/view/microsoft-net-architecting/9780133986419/ch10.html">Microsoft .NET: Architecting Applications for the Enterprise, Second Edition: Chapter 10. Introducing CQRS</seealso>
    public class ReadModelFacade : EntityFrameworkReadModelFacade
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadModelFacade"/> class.
        /// </summary>
        /// <param name="context">The current <see cref="DataContext"/> instance.</param>
        public ReadModelFacade(DataContext context)
            : base(context)
        {
        }
    }
}