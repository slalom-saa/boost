using System;
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
        private static readonly string endpointUrl = "https://patolus-documents.documents.azure.com:443/";
        private static readonly string authorizationKey = "ASdxo55GhyAEXKFCyK21kkQpsu09XTmb6mYmrJhxe0hyllq7b7jfCuhSeZ6JrmPIQvfAcQxWtL8IJkLJIjY4Qw==";
        private static readonly string databaseName = "treatment";
        private static readonly string collectionName = "entries";
        private DocumentClient client;

        Uri collectionUri = UriFactory.CreateDocumentCollectionUri(databaseName, collectionName);

        public DocumentDbRepository()
        {
            client = GetClient();
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

            return new DocumentClient(new Uri(endpointUrl), authorizationKey, connectionPolicy);
        }

        public virtual void Delete()
        {
            var items = client.CreateDocumentQuery<DocumentItem<TRoot>>(collectionUri)
                              .Select(e => e.Id)
                              .ToList();
            foreach (var item in items)
            {
                client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, item.ToString())).Wait();
            }
        }

        public virtual void Delete(TRoot[] instances)
        {
            foreach (var item in instances)
            {
                client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, item.Id.ToString())).Wait();
            }
        }

        public virtual TRoot Find(Guid id)
        {
            return client.CreateDocumentQuery<DocumentItem<TRoot>>(collectionUri)
                         .Where(e => e.Id == id)
                         .Select(e => e.Value)
                         .ToList()
                         .FirstOrDefault();
        }

        public virtual IQueryable<TRoot> Find()
        {
            return client.CreateDocumentQuery<DocumentItem<TRoot>>(collectionUri)
                         .Select(e => e.Value);
        }

        public virtual void Add(TRoot[] instances)
        {
            foreach (var instance in instances)
            {
                client.CreateDocumentAsync(collectionUri, new DocumentItem<TRoot>(instance)).Wait();
            }
        }

        public virtual void Update(TRoot[] instances)
        {
            foreach (var instance in instances)
            {
                client.UpsertDocumentAsync(collectionUri, new DocumentItem<TRoot>(instance)).Wait();
            }
        }
    }
}