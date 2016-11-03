using System;
using System.Configuration;
using System.Linq;

namespace Slalom.Boost.DocumentDb
{
    public class DocumentDbOptions
    {
        public string ServiceEndpoint { get; set; } = ConfigurationManager.AppSettings["DocumentDB:ServiceEndpoint"];

        public string AuthorizationKey { get; set; } = ConfigurationManager.AppSettings["DocumentDB:AuthorizationKey"];

        public string DatabaseId { get; set; } = ConfigurationManager.AppSettings["DocumentDB:DatabaseId"];

        public string CollectionId { get; set; } = ConfigurationManager.AppSettings["DocumentDB:CollectionId"];
    }
}
