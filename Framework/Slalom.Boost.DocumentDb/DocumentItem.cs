using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Slalom.Boost.Domain;

namespace Slalom.Boost.DocumentDb
{
    public class DocumentItem<TRoot> where TRoot : IAggregateRoot
    {
        [JsonProperty("id")]
        public Guid Id { get; private set; }

        public string PartitionKey { get; private set; }

        public TRoot Value { get; private set; }

        protected DocumentItem()
        {
        }

        public DocumentItem(TRoot item)
        {
            this.Value = item;
            this.Id = item.Id;
            this.PartitionKey = typeof(TRoot).Name;
        }
    }
}
