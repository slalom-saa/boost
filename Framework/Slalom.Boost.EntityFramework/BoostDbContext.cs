using System;
using System.Data.Entity;

namespace Slalom.Boost.EntityFramework
{
    public abstract class BoostDbContext : DbContext
    {
        protected BoostDbContext(string connection)
            : base(connection)
        {
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
        }

        protected BoostDbContext()
        {
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromAssembly(typeof(BoostDbContext).Assembly);
        }
    }
}