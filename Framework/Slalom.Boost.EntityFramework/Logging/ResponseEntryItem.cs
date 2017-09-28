using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slalom.Boost.EntityFramework.Logging
{
    public class ResponseEntryItem
    {
        [StringLength(255)]
        public string ApplicationName { get; set; }

        [StringLength(100)]
        public string Build { get; set; }

        [StringLength(255)]
        public string Channel { get; set; }

        public DateTimeOffset? Completed { get; set; }

        [StringLength(100)]
        public string CorrelationId { get; set; }

        public TimeSpan Elapsed { get; set; }

        [StringLength(100)]
        public string EntryId { get; set; }

        [StringLength(255)]
        public string Environment { get; set; }

        [StringLength(4000)]
        public string Exception { get; set; }

        [StringLength(1000)]
        public string Function { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool IsSuccessful { get; set; }

        [StringLength(255)]
        public string MachineName { get; set; }

        [StringLength(255)]
        public string Path { get; set; }

        [StringLength(100)]
        public string RequestId { get; set; }

        public DateTimeOffset Started { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        [StringLength(4000)]
        public string ValidationErrors { get; set; }

        [StringLength(100)]
        public string Version { get; set; }

        public bool Cancelled { get; set; }
    }
}