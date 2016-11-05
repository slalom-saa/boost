using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slalom.Boost.MongoDB
{
    public class MongoDbOptions
    {
        public string Database { get; set; } = ConfigurationManager.AppSettings["MongoDB:Database"];

        public string UserName { get; set; } = ConfigurationManager.AppSettings["MongoDB:UserName"];

        public string Password { get; set; } = ConfigurationManager.AppSettings["MongoDB:Password"];

        public string Server { get; set; } = ConfigurationManager.AppSettings["MongoDB:Server"];

        public int Port { get; set; } = Convert.ToInt32(ConfigurationManager.AppSettings["MongoDB:Port"]);

        public bool UseSsl { get; set; } = Convert.ToBoolean(ConfigurationManager.AppSettings["MongoDB:UseSsl"] ?? "false");
    }
}
