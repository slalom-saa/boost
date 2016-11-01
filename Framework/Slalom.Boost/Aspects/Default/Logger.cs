using System;
using Slalom.Boost.Events;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Aspects.Default
{
    /// <summary>
    /// Provides default <see cref="ILogger"/> functionality.
    /// </summary>
    /// <seealso cref="Slalom.Boost.Aspects.ILogger" />
    [DefaultBinding]
    public class Logger : ILogger
    {
        private readonly IApplicationBus _bus;
        private readonly IExecutionContextResolver _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="context">The context.</param>
        public Logger(IApplicationBus bus, IExecutionContextResolver context)
        {
            _bus = bus;
            _context = context;
        }

        /// <summary>
        /// Writes the specified debug message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="data">Additional data to write to the log.</param>
        public void Debug(object message, params object[] data)
        {
            _bus.Publish(new ApplicationEvent(message, _context.Resolve(), LogSeverity.Debug, data)).Wait();
        }

        /// <summary>
        /// Writes the specified error message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="data">Additional data to write to the log.</param>
        public void Error(object message, params object[] data)
        {
            _bus.Publish(new ApplicationEvent(message, _context.Resolve(), LogSeverity.Error, data)).Wait();
        }

        /// <summary>
        /// Writes the specified information message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="data">Additional data to write to the log.</param>
        public void Information(object message, params object[] data)
        {
            _bus.Publish(new ApplicationEvent(message, _context.Resolve(), LogSeverity.Information, data)).Wait();
        }

        /// <summary>
        /// Writes the specified warning message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="data">Additional data to write to the log.</param>
        public void Warning(object message, params object[] data)
        {
            _bus.Publish(new ApplicationEvent(message, _context.Resolve(), LogSeverity.Warning, data)).Wait();
        }
    }
}