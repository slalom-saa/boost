using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Slalom.Boost.Domain;
using Slalom.Boost.Logging;
using Slalom.Boost.RuntimeBinding;
using Slalom.Boost.Serialization;

namespace Slalom.Boost.DocumentDb
{
    [RuntimeBindingImplementation(ImplementationBindingType.Singleton)]
    public class DocumentDbContext : IDisposable
    {
        private readonly Lazy<DocumentClient> Client;

        public DocumentDbContext(DocumentDbOptions options)
        {
            this.Options = options;

            Client = new Lazy<DocumentClient>(() => this.GetClient());

            JsonConvert.DefaultSettings = () =>
            {
                return new DefaultSerializationSettings();
            };
        }

        /// <summary>
        /// Gets or sets the configured <see cref="ILogger"/> instance.
        /// </summary>
        /// <value>The configured <see cref="ILogger"/> instance.</value>
        [RuntimeBindingDependency]
        public ILogger Logger { get; set; }

        public DocumentDbOptions Options { get; }

        protected Uri CollectionUri => UriFactory.CreateDocumentCollectionUri(this.Options.DatabaseId, this.Options.CollectionId);

        public virtual void Add<TRoot>(TRoot[] instances, int batchSize) where TRoot : IAggregateRoot
        {
            var current = instances.ToList();
            while (current.Any())
            {
                this.InvokeBulkImportSproc(current.Take(batchSize)).Wait();
                current = current.Skip(batchSize).ToList();
            }
        }

        public virtual void Add<TRoot>(TRoot[] instances) where TRoot : IAggregateRoot
        {
            if (instances.Count() == 1)
            {
                Client.Value.CreateDocumentAsync(this.CollectionUri, new DocumentItem<TRoot>(instances[0]));
            }
            else
            {
                this.InvokeBulkImportSproc(instances).Wait();
            }
        }

        public virtual void Delete<TRoot>() where TRoot : IAggregateRoot
        {
            this.InvokeBulkDeleteSproc<TRoot>().Wait();
        }

        public virtual void Delete<TRoot>(TRoot[] instances) where TRoot : IAggregateRoot
        {
            foreach (var item in instances)
            {
                Client.Value.DeleteDocumentAsync(UriFactory.CreateDocumentUri(this.Options.DatabaseId, this.Options.CollectionId, item.Id.ToString())).Wait();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual IQueryable<TRoot> Find<TRoot>() where TRoot : IAggregateRoot
        {
            return Client.Value.CreateDocumentQuery<DocumentItem<TRoot>>(this.CollectionUri)
                         .Where(e => e.PartitionKey == typeof(TRoot).Name)
                         .Select(e => e.Value);
        }

        public virtual TRoot Find<TRoot>(Guid id) where TRoot : IAggregateRoot
        {
            var response = Client.Value.ReadDocumentAsync(UriFactory.CreateDocumentUri(this.Options.DatabaseId, this.Options.CollectionId, id.ToString())).Result;

            return (TRoot)(dynamic)response.Resource;
        }

        public virtual void Update<TRoot>(TRoot[] instances) where TRoot : IAggregateRoot
        {
            if (instances.Count() == 1)
            {
                Client.Value.UpsertDocumentAsync(CollectionUri, new DocumentItem<TRoot>(instances[0]));
            }
            else
            {
                this.InvokeBulkImportSproc(instances).Wait();
            }
        }

        public virtual void Update<TRoot>(TRoot[] instances, int batchSize) where TRoot : IAggregateRoot
        {
            var current = instances.ToList();
            while (current.Any())
            {
                this.InvokeBulkImportSproc(current.Take(batchSize)).Wait();
                current = current.Skip(batchSize).ToList();
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Client.IsValueCreated)
                {
                    Client.Value.Dispose();
                }
            }
        }

        protected Task InvokeBulkDeleteSproc<TRoot>() where TRoot : IAggregateRoot
        {
            var scriptName = "bulkDelete";

            var sprocUri = UriFactory.CreateStoredProcedureUri(this.Options.DatabaseId, this.Options.CollectionId, scriptName);

            return Client.Value.ExecuteStoredProcedureAsync<Document>(sprocUri, typeof(TRoot).Name);
        }

        protected Task InvokeBulkImportSproc<TRoot>(IEnumerable<TRoot> instances) where TRoot : IAggregateRoot
        {
            var scriptName = "bulkImport";

            var sprocUri = UriFactory.CreateStoredProcedureUri(this.Options.DatabaseId, this.Options.CollectionId, scriptName);

            return Client.Value.ExecuteStoredProcedureAsync<int>(sprocUri, instances.Select(e => new DocumentItem<TRoot>(e)), true);
        }

        private DocumentClient GetClient()
        {
            var client = new DocumentClient(new Uri(this.Options.ServiceEndpoint), this.Options.AuthorizationKey, new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            });
            client.OpenAsync().Wait();
            return client;
        }
    }
}