using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slalom.Boost.EntityFramework.Logging
{
    public class EventEntryItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string EventId { get; set; }

        [StringLength(255)]
        public string ApplicationName { get; set; }

        public string Event { get; set; }

        [StringLength(255)]
        public string Environment { get; set; }

        [StringLength(1000)]
        public string EventType { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(100)]
        public string RequestId { get; set; }

        public DateTimeOffset TimeStamp { get; set; }
    }
}
