using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Principal;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Aspects.Default
{
    /// <summary>
    /// A <see cref="IExecutionContextResolver"/> implementation for Windows-based applications.
    /// </summary>
    /// <seealso cref="IExecutionContextResolver" />
    [DefaultBinding]
    public sealed class WindowsExecutionContextResolver : IExecutionContextResolver
    {
        private static Guid sessionId = Guid.NewGuid();

        /// <summary>
        /// Resolves the execution context.
        /// </summary>
        /// <returns>Returns the resolved execution context.</returns>
        public ExecutionContext Resolve()
        {
            var context = new ExecutionContext(WindowsIdentity.GetCurrent(), sessionId.ToString());
            context.Data.Add("Client", Environment.OSVersion?.ToString());
            context.Data.Add("MachineName", Environment.MachineName);
            context.Data.Add("Local IP", GetLocalIpAddress());
            context.Data.Add("EntryAssembly", Assembly.GetEntryAssembly()?.FullName);
            context.Data.Add("Host", "Windows");
            return context;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "As designed.")]
        private static string GetLocalIpAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }
            catch
            {
            }
            return "Local IP Address Not Found!";
        }
    }
}