﻿using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Web;

namespace Slalom.Boost.Aspects
{
    /// <summary>
    /// Represents an execution context and contains information about the session and user.
    /// </summary>
    public class ExecutionContext
    {
        private const string Key = "CorrelationId";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionContext"/> class.
        /// </summary>
        /// <param name="identity">The current identity.</param>
        /// <param name="session">The current session.</param>
        public ExecutionContext(IIdentity identity, string session)
        {
            this.Identity = identity;
            this.Session = session;
        }

        /// <summary>
        /// Gets the identity being used during execution.
        /// </summary>
        /// <value>The identity.</value>
        public IIdentity Identity { get; private set; }

        /// <summary>
        /// Gets the execution session.
        /// </summary>
        /// <value>The session.</value>
        public string Session { get; private set; }

        /// <summary>
        /// Gets any additional information about the execution context.
        /// </summary>
        /// <value>Any additional information about the execution context.</value>
        public Dictionary<string, string> Data { get; } = new Dictionary<string, string>();

        /// <summary>
        /// The correlation identifier used to trace along an execution path.
        /// </summary>
        public Guid CorrelationId { get; } = GetCorrelationId();

        private static Guid GetCorrelationId()
        {
            if (HttpContext.Current != null)
            {
                if (!HttpContext.Current.Items.Contains(Key))
                {
                    HttpContext.Current.Items.Add(Key, Guid.NewGuid());
                }
                return (Guid)HttpContext.Current.Items[Key];
            }
            if (CallContext.GetData(Key) == null)
            {
                var id = Guid.NewGuid();
                CallContext.SetData(Key, id);
            }
            return new Guid(CallContext.GetData(Key).ToString());
        }
    }
}