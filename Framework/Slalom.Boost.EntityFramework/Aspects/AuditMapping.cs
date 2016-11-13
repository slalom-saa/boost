using System;
using System.Data.Entity.ModelConfiguration;
using Slalom.Boost.Commands;

namespace Slalom.Boost.EntityFramework.Aspects
{
    /// <summary>
    /// Contains the Entity Framework mapping for the <see cref="CommandAudit" /> class.
    /// </summary>
    public class AuditMapping : EntityTypeConfiguration<CommandAudit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditMapping"/> class.
        /// </summary>
        public AuditMapping()
        {		
			// Table
            this.ToTable("Audits");

			// Key
            this.HasKey(e => e.Id);
        }
    }

    /// <summary>
    /// Contains the Entity Framework mapping for the <see cref="CommandAudit" /> class.
    /// </summary>
    public class EventMapping : EntityTypeConfiguration<EventAudit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditMapping"/> class.
        /// </summary>
        public EventMapping()
        {
            // Table
            this.ToTable("Events");

            // Key
            this.HasKey(e => e.Id);
        }
    }
}