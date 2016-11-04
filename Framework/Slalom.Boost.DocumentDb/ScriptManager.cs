using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Slalom.Boost.DocumentDb
{
    public class CollectionManger
    {
        private readonly DocumentDbOptions _options;

        public CollectionManger(DocumentDbOptions options)
        {
            _options = options;
        }

        public void ResetDatabase()
        {
            using (var client = new DocumentClient(new Uri(_options.ServiceEndpoint), _options.AuthorizationKey))
            {

                Database database = client.CreateDatabaseQuery()
                                          .Where(db => db.Id == _options.DatabaseId)
                                          .ToArray()
                                          .FirstOrDefault();

                if (database != null)
                {
                    client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(_options.DatabaseId)).Wait();
                }

                client.CreateDatabaseAsync(new Database { Id = _options.DatabaseId });

                DocumentCollection collectionDefinition = new DocumentCollection();
                collectionDefinition.Id = _options.CollectionId;

                // For this demo, we create a collection to store SalesOrders. We set the partition key to the account
                // number so that we can retrieve all sales orders for an account efficiently from a single partition,
                // and perform transactions across multiple sales order for a single account number. 
                //collectionDefinition.PartitionKey.Paths.Add("/PartitionKey");

                // Use the recommended indexing policy which supports range queries/sorting on strings
                collectionDefinition.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

                // Create with a throughput of 1000 RU/s
                client.CreateDocumentCollectionAsync(
                    UriFactory.CreateDatabaseUri(_options.DatabaseId),
                    collectionDefinition,
                    new RequestOptions { OfferThroughput = 1000 }).Wait();
            }
        }
    }

    public class ScriptManager
    {
        private readonly DocumentDbOptions _options;
        private bool _initialized;

        public ScriptManager(DocumentDbOptions options)
        {
            _options = options;
        }

        public void EnsureScripts()
        {
            if (!_initialized)
            {
                _initialized = true;

                Task.WhenAll(
                    this.EnsureProcedure("bulkImport", Scripts.bulkImport),
                    this.EnsureProcedure("bulkDelete", Scripts.bulkDelete)
                    ).Wait();
            }
        }

        private async Task EnsureProcedure(string scriptName, string content)
        {
            using (var client = new DocumentClient(new Uri(_options.ServiceEndpoint), _options.AuthorizationKey))
            {
                var collectionLink = UriFactory.CreateDocumentCollectionUri(_options.DatabaseId, _options.CollectionId);

                var sproc = new StoredProcedure
                {
                    Id = scriptName,
                    Body = content
                };

                var sprocUri = UriFactory.CreateStoredProcedureUri(_options.DatabaseId, _options.CollectionId, scriptName);

                try
                {
                    await client.ReadStoredProcedureAsync(sprocUri);
                }
                catch (DocumentClientException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
                {
                    await client.CreateStoredProcedureAsync(collectionLink, sproc);
                }
            }
        }
    }
}