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
    /// <summary>
    /// Provides a DocumentDB based <see cref="IRepository{TRoot}"/> implementation.
    /// </summary>
    /// <typeparam name="TRoot">The type of aggregate root to use.</typeparam>
    /// <seealso cref="Slalom.Boost.Domain.IRepository{TEntity}" />
    public abstract class DocumentDbRepository<TRoot> : IRepository<TRoot> where TRoot : class, IAggregateRoot
    {
        private Lazy<DocumentClient> Client;

        public DocumentDbRepository()
        {
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

        private Uri collectionUri => UriFactory.CreateDocumentCollectionUri(this.Options.DatabaseId, this.Options.CollectionId);

        [RuntimeBindingDependency]
        public DocumentDbOptions Options { get; set; }

        public virtual void Delete()
        {
            this.Logger.Verbose("Deleting all items of type {Type} using {Repository}.", typeof(TRoot).Name, this.GetType().BaseType);

            this.InvokeBulkDeleteSproc().Wait();
        }

        public virtual void Delete(TRoot[] instances)
        {
            this.Logger.Verbose("Deleting {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            foreach (var item in instances)
            {
                Client.Value.DeleteDocumentAsync(UriFactory.CreateDocumentUri(this.Options.DatabaseId, this.Options.CollectionId, item.Id.ToString())).Wait();
            }
        }

        public virtual TRoot Find(Guid id)
        {
            this.Logger.Verbose("Finding item of type {Type} with ID {Id} using {Repository}.", typeof(TRoot).Name, id, this.GetType().BaseType);

            return Client.Value.CreateDocumentQuery<DocumentItem<TRoot>>(this.collectionUri)
                         .Where(e => e.Id == id)
                         .Select(e => e.Value)
                         .ToList()
                         .FirstOrDefault();
        }

        public virtual IQueryable<TRoot> Find()
        {
            this.Logger.Verbose("Finding all items of type {Type} using {Repository}.", typeof(TRoot).Name, this.GetType().BaseType);

            return Client.Value.CreateDocumentQuery<DocumentItem<TRoot>>(this.collectionUri)
                         .Where(e => e.PartitionKey == typeof(TRoot).Name)
                         .Select(e => e.Value);
        }

        public virtual void Add(TRoot[] instances)
        {
            this.Logger.Verbose("Adding {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            this.InvokeBulkImportSproc(instances).Wait();
        }

        protected virtual void Add(TRoot[] instances, int batchSize)
        {
            this.Logger.Verbose("Adding {Count} items of type {Type} using {Repository}.  Using a batch size of {Size}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType, batchSize);

            var current = instances.ToList();
            while (current.Any())
            {
                this.InvokeBulkImportSproc(current.Take(batchSize)).Wait();
                current = current.Skip(batchSize).ToList();
            }
        }

        public virtual void Update(TRoot[] instances)
        {
            this.Logger.Verbose("Updating {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            this.InvokeBulkImportSproc(instances).Wait();
        }

        protected virtual void Update(TRoot[] instances, int batchSize)
        {
            this.Logger.Verbose("Updating {Count} items of type {Type} using {Repository}.  Using a batch size of {Size}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType, batchSize);

            var current = instances.ToList();
            while (current.Any())
            {
                this.InvokeBulkImportSproc(current.Take(batchSize)).Wait();
                current = current.Skip(batchSize).ToList();
            }
        }

        private DocumentClient GetClient()
        {
            var connectionPolicy = new ConnectionPolicy();
            connectionPolicy.UserAgentSuffix = " samples-net/3";
            connectionPolicy.ConnectionMode = ConnectionMode.Direct;
            connectionPolicy.ConnectionProtocol = Protocol.Tcp;

            // Set the read region selection preference order
            connectionPolicy.PreferredLocations.Add(LocationNames.WestUS); // first preference
            connectionPolicy.PreferredLocations.Add(LocationNames.EastUS); // second preference

            return new DocumentClient(new Uri(this.Options.ServiceEndpoint), this.Options.AuthorizationKey, connectionPolicy);
        }

        private Task InvokeBulkImportSproc(IEnumerable<TRoot> instances)
        {
            var scriptName = "bulkImport";

            var sprocUri = UriFactory.CreateStoredProcedureUri(this.Options.DatabaseId, this.Options.CollectionId, scriptName);

            return Client.Value.ExecuteStoredProcedureAsync<int>(sprocUri, instances.Select(e => new DocumentItem<TRoot>(e)), true);
        }

        private Task InvokeBulkDeleteSproc()
        {
            var scriptName = "bulkDelete";

            var sprocUri = UriFactory.CreateStoredProcedureUri(this.Options.DatabaseId, this.Options.CollectionId, scriptName);

            return Client.Value.ExecuteStoredProcedureAsync<Document>(sprocUri, typeof(TRoot).Name);
        }
    }
}