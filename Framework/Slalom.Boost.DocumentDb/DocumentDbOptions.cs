using System;
using System.Configuration;
using System.Linq;

namespace Slalom.Boost.DocumentDb
{
    /// <summary>
    /// Contains options needed to configure a DocumentDB connection.  The default implemenation is to use appsettings:
    /// DocumentDB:ServiceEndpoint, DocumentDB:AuthorizationKey, DocumentDB:DatabaseId, DocumentDB:CollectionId.
    /// </summary>
    public class DocumentDbOptions
    {
        /// <summary>
        /// Gets or sets the authorization key.
        /// </summary>
        /// <value>The authorization key.</value>
        public string AuthorizationKey { get; set; } = ConfigurationManager.AppSettings["DocumentDB:AuthorizationKey"];

        /// <summary>
        /// Gets or sets the collection identifier.
        /// </summary>
        /// <value>The collection identifier.</value>
        public string CollectionId { get; set; } = ConfigurationManager.AppSettings["DocumentDB:CollectionId"];

        /// <summary>
        /// Gets or sets the database identifier.
        /// </summary>
        /// <value>The database identifier.</value>
        public string DatabaseId { get; set; } = ConfigurationManager.AppSettings["DocumentDB:DatabaseId"];

        /// <summary>
        /// Gets or sets the service endpoint.
        /// </summary>
        /// <value>The service endpoint.</value>
        public string ServiceEndpoint { get; set; } = ConfigurationManager.AppSettings["DocumentDB:ServiceEndpoint"];
    }
}