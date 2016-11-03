using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Slalom.Boost.DocumentDb
{
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

                Task.WhenAll(this.EnsureProcedure("bulkImport", Scripts.bulkImport)).Wait();
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