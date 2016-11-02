using System;
using System.Configuration;
using System.Linq;

namespace Slalom.Boost.DocumentDb
{
    public class DocumentDbOptions
    {
        public string Host { get; set; } = ConfigurationManager.AppSettings["DocumentDB:Host"];

        public int Port { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["DocumentDB:Port"] ?? "10250");

        public string Database { get; set; } = ConfigurationManager.AppSettings["DocumentDB:Database"];

        public string UserName { get; set; } = ConfigurationManager.AppSettings["DocumentDB:UserName"];

        public string Password { get; set; } = ConfigurationManager.AppSettings["DocumentDB:Password"];

        public string Collection { get; set; } = ConfigurationManager.AppSettings["DocumentDB:Collection"];
    }
}
