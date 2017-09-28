using System;
using System.Data.Entity;

namespace Slalom.Boost.EntityFramework.Logging
{
   
    public class LoggingDbContext : DbContext
    {
        public LoggingDbContext() : this(new SqlServerLoggingOptions())
        {
        }

        private readonly SqlServerLoggingOptions _options;

        static LoggingDbContext()
        {
            //LicenseManager.AddLicense("1260;101-Slalom", "5d118d76-80c0-4eb3-543b-038a6d86ff55");

            Database.SetInitializer<LoggingDbContext>(null);
        }

        public DbSet<RequestEntryItem> Requests { get; set; }

        public DbSet<ResponseEntryItem> Responses { get; set; }

        public DbSet<EventEntryItem> Events { get; set; }

        public LoggingDbContext(SqlServerLoggingOptions options) : base(options.ConnectionString)
        {
            _options = options;

            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RequestEntryItem>().ToTable(_options.RequestsTableName, _options.Schema);
            modelBuilder.Entity<ResponseEntryItem>().ToTable(_options.ResponsesTableName, _options.Schema);
            modelBuilder.Entity<EventEntryItem>().ToTable(_options.EventsTableName, _options.Schema);
        }
    }
}
