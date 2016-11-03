using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Slalom.Boost.Domain;

namespace Slalom.Boost.DocumentDb
{
    /// <summary>
    /// Provides a DocumentDB based <see cref="IRepository{TRoot}"/> implementation.
    /// </summary>
    /// <typeparam name="TRoot">The type of aggregate root to use.</typeparam>
    /// <seealso cref="Slalom.Boost.Domain.IRepository{TEntity}" />
    public abstract class DocumentDbRepository<TRoot> : IRepository<TRoot> where TRoot : class, IAggregateRoot
    {

        private Uri collectionUri => UriFactory.CreateDocumentCollectionUri(Options.DatabaseId, Options.CollectionId);

        [RuntimeBinding.RuntimeBindingDependency]
        public DocumentDbOptions Options { get; set; }


        private Lazy<DocumentClient> Client;

        public DocumentDbRepository()
        {
            Client = new Lazy<DocumentClient>(() => GetClient());
        }
        private DocumentClient GetClient()
        {
            ConnectionPolicy connectionPolicy = new ConnectionPolicy();
            connectionPolicy.UserAgentSuffix = " samples-net/3";
            connectionPolicy.ConnectionMode = ConnectionMode.Direct;
            connectionPolicy.ConnectionProtocol = Protocol.Tcp;

            // Set the read region selection preference order
            connectionPolicy.PreferredLocations.Add(LocationNames.WestUS); // first preference
            connectionPolicy.PreferredLocations.Add(LocationNames.EastUS); // second preference

            return new DocumentClient(new Uri(Options.ServiceEndpoint), Options.AuthorizationKey, connectionPolicy);
        }

        public virtual void Delete()
        {
            var items = Client.Value.CreateDocumentQuery<DocumentItem<TRoot>>(collectionUri)
                              .Select(e => e.Id)
                              .ToList();

            foreach (var item in items)
            {
                Client.Value.DeleteDocumentAsync(UriFactory.CreateDocumentUri(Options.DatabaseId, Options.CollectionId, item.ToString("D").ToLower())).Wait();
            }
        }

        public virtual void Delete(TRoot[] instances)
        {
            foreach (var item in instances)
            {
                Client.Value.DeleteDocumentAsync(UriFactory.CreateDocumentUri(Options.DatabaseId, Options.CollectionId, item.Id.ToString())).Wait();
            }
        }

        public virtual TRoot Find(Guid id)
        {
            return Client.Value.CreateDocumentQuery<DocumentItem<TRoot>>(collectionUri)
                         .Where(e => e.Id == id)
                         .Select(e => e.Value)
                         .ToList()
                         .FirstOrDefault();
        }

        public virtual IQueryable<TRoot> Find()
        {
            return Client.Value.CreateDocumentQuery<DocumentItem<TRoot>>(collectionUri)
                         .Select(e => e.Value);
        }

        public virtual void Add(TRoot[] instances)
        {
            Console.WriteLine("Adding " + typeof(TRoot).Name);
            InvokeBulkImportSproc(instances).Wait();
        }

        private async Task InvokeBulkImportSproc(IEnumerable<TRoot> instances)
        {
            string scriptName = "bulkImport";

            Uri sprocUri = UriFactory.CreateStoredProcedureUri(Options.DatabaseId, Options.CollectionId, scriptName);

            await Client.Value.ExecuteStoredProcedureAsync<int>(sprocUri, instances.Select(e => new DocumentItem<TRoot>(e)), true);
        }

        public virtual void Update(TRoot[] instances)
        {
            foreach (var instance in instances)
            {
                Client.Value.UpsertDocumentAsync(collectionUri, new DocumentItem<TRoot>(instance)).Wait();
            }
        }
    }
}