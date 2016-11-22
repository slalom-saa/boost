using System;

namespace Slalom.Boost.Logging
{
    /// <summary>
    /// A null <see cref="ILogger"/> implementation.
    /// </summary>
    /// <seealso cref="ILogger" />
    public class NullLogger : ILogger
    {
        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="template">Message template describing the event.</param>
        /// <param name="properties">Objects positionally formatted into the message template.</param>
        public void Debug(Exception exception, string template, params object[] properties)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="template">Message template describing the event.</param>
        /// <param name="properties">Objects positionally formatted into the message template.</param>
        public void Debug(string template, params object[] properties)
        {
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="template">Message template describing the event.</param>
        /// <param name="properties">Objects positionally formatted into the message template.</param>
        public void Error(Exception exception, string template, params object[] properties)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="template">Message template describing the event.</param>
        /// <param name="properties">Objects positionally formatted into the message template.</param>
        public void Error(string template, params object[] properties)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="template">Message template describing the event.</param>
        /// <param name="properties">Objects positionally formatted into the message template.</param>
        public void Fatal(Exception exception, string template, params object[] properties)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="template">Message template describing the event.</param>
        /// <param name="properties">Objects positionally formatted into the message template.</param>
        public void Fatal(string template, params object[] properties)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="template">Message template describing the event.</param>
        /// <param name="properties">Objects positionally formatted into the message template.</param>
        public void Information(Exception exception, string template, params object[] properties)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="template">Message template describing the event.</param>
        /// <param name="properties">Objects positionally formatted into the message template.</param>
        public void Information(string template, params object[] properties)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="template">Message template describing the event.</param>
        /// <param name="properties">Objects positionally formatted into the message template.</param>
        public void Verbose(Exception exception, string template, params object[] properties)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="template">Message template describing the event.</param>
        /// <param name="properties">Objects positionally formatted into the message template.</param>
        public void Verbose(string template, params object[] properties)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="template">Message template describing the event.</param>
        /// <param name="properties">Objects positionally formatted into the message template.</param>
        public void Warning(Exception exception, string template, params object[] properties)
        {
        }

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="template">Message template describing the event.</param>
        /// <param name="properties">Objects positionally formatted into the message template.</param>
        public void Warning(string template, params object[] properties)
        {
        }
    }
}