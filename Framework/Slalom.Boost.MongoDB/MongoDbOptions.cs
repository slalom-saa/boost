using System;
using System.Configuration;
using System.Linq;

namespace Slalom.Boost.MongoDB
{
    /// <summary>
    /// Contains options needed to configure a MongoDB connection.  The default implemenation is to use appsettings:
    /// MongoDB:Database, MongoDB:UserName, MongoDB:Password, MongoDB:Server, MongoDB:Port, MongoDB:UseSsl.
    /// </summary>
    public class MongoDbOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbOptions"/> class.
        /// </summary>
        public MongoDbOptions()
        {
            this.Database = ConfigurationManager.AppSettings["MongoDB:Database"];
            this.UserName = ConfigurationManager.AppSettings["MongoDB:UserName"];
            this.Password = ConfigurationManager.AppSettings["MongoDB:Password"];
            this.Server = ConfigurationManager.AppSettings["MongoDB:Server"];
            this.Port = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["MongoDB:Port"]) ? Convert.ToInt32(ConfigurationManager.AppSettings["MongoDB:Port"]) : 27017;
            this.UseSsl = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["MongoDB:UseSsl"]) && Convert.ToBoolean(ConfigurationManager.AppSettings["MongoDB:UseSsl"]);
        }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>The database.</value>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>The server.</value>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use SSL].
        /// </summary>
        /// <value><c>true</c> if [use SSL]; otherwise, <c>false</c>.</value>
        public bool UseSsl { get; set; }
    }
}