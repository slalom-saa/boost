/* 
 * Copyright (c) Patolus Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Configuration;

namespace Slalom.Boost.EntityFramework.Logging
{
    /// <summary>
    /// Options for SQL Server Logging.
    /// </summary>
    public class SqlServerLoggingOptions
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; } = GetSetting("ConnectionString", "Data Source=.;Initial Catalog=Patolus.Logs;Integrated Security=True;MultipleActiveResultSets=True");

        /// <summary>
        /// Gets or sets the name of the table that is used for events.
        /// </summary>
        /// <value>The name of the table that is used for events.</value>
        public string EventsTableName { get; set; } = GetSetting("EventsTableName", "Events");

        /// <summary>
        /// Gets or sets the name of the table that is used for requests.
        /// </summary>
        /// <value>The name of the table that is used for requests.</value>
        public string RequestsTableName { get; set; } = GetSetting("RequestsTableName", "Requests");

        /// <summary>
        /// Gets or sets the name of the table that is used for responses.
        /// </summary>
        /// <value>The name of the table that is used for responses.</value>
        public string ResponsesTableName { get; set; } = GetSetting("ResponsesTableName", "Responses");

        public string Schema { get; set; } = GetSetting("Schema", "dbo");

        /// <summary>
        /// Gets the trace log level.
        /// </summary>
        /// <value>The trace log level.</value>
        public string TraceLogLevel { get; set; } = GetSetting("TraceLogLevel", "Warning");

        /// <summary>
        /// Gets or sets the name of the table that is used for traces.
        /// </summary>
        /// <value>The name of the table that is used for traces.</value>
        public string TraceTableName { get; set; } = GetSetting("TraceTableName", "Traces");

        private static int GetSetting(string name, int value)
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["SqlLogging:" + name] ?? value.ToString());
        }

        private static string GetSetting(string name, string value)
        {
            return ConfigurationManager.AppSettings["SqlLogging:" + name] ?? value;
        }
    }
}