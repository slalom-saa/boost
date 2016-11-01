using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slalom.Boost.EntityFramework;

namespace $safeprojectname$
{
    // TODO: Choose the appropriate initializer
    // TODO: Split into a read and command data context when appropriate
    
    /// <summary>
    /// Provides a common <see cref="DbContext">EF DB Context</see> for the module.
    /// </summary>
    public class DataContext : BoostDbContext
{
    /// <summary>
    /// Initializes the <see cref="DataContext"/> class.
    /// </summary>
    static DataContext()
        {
#warning Choose the appropriate initialization strategy

        // Drop when the database model changes
         Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataContext>());


        // or drop create always
        // Database.SetInitializer(new DropCreateDatabaseAlways<DataContext>());


        // or set to null to avoid migration overhead
        // Database.SetInitializer<DataContext>(null);

        // or to enable migrations and seeding:
        // run enable-migrations in the Package Manager Console
        // and add the following
        // Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataContext"/> class.
    /// </summary>
    public DataContext() 
         : base("Name=________")
        {
    }

    /// <summary>
    /// This method is called when the model for a derived context has been initialized, but
    /// before the model has been locked down and used to initialize the context.  The default
    /// implementation of this method does nothing, but it can be overridden in a derived class
    /// such that the model can be further configured before it is locked down.
    /// </summary>
    /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
    /// <remarks>
    /// Typically, this method is called only once when the first instance of a derived context
    /// is created.  The model for that context is then cached and is for all further instances of
    /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
    /// property on the given ModelBuidler, but note that this can seriously degrade performance.
    /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
    /// classes directly.
    /// </remarks>
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromAssembly(this.GetType().Assembly);
        }
    }
}
