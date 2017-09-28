using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slalom.Boost.EntityFramework.Logging
{
    public class RequestEntryItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string EntryId { get; set; }

        [StringLength(100)]
        public string CorrelationId { get; set; }

        public string Body { get; set; }

        [StringLength(100)]
        public string RequestId { get; set; }

        [StringLength(1000)]
        public string RequestType { get; set; }

        [StringLength(100)]
        public string Parent { get; set; }

        [StringLength(255)]
        public string Path { get; set; }

        [StringLength(255)]
        public string Channel { get; set; }

        [StringLength(100)]
        public string SessionId { get; set; }

        [StringLength(100)]
        public string SourceAddress { get; set; }

        public DateTimeOffset? TimeStamp { get; set; }

        [StringLength(255)]
        public string UserName { get; set; }

        [StringLength(255)]
        public string ApplicationName { get; set; }

        [StringLength(255)]
        public string Environment { get; set; }

        [StringLength(255)]
        public string MachineName { get; set; }
    }
}
