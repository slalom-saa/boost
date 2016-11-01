using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Slalom.Boost.Commands
{
    /// <summary>
    /// The exception that is raised when executing a command chain.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class CommandExecutionFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutionFailedException"/> class.
        /// </summary>
        /// <param name="commandId">The command being processed.</param>
        public CommandExecutionFailedException(Guid commandId)
            : base("An exception occurred while executing a command: " + commandId)
        {
            this.CommandId = commandId;
        }

        /// <summary>
        /// Gets the identifier of the command that raised the exception.
        /// </summary>
        /// <value>The identifier of the command that raised the exception.</value>
        public Guid CommandId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExecutionFailedException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected CommandExecutionFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.CommandId = new Guid(info.GetString("CommandId"));
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
        /// </PermissionSet>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("CommandId", this.CommandId);
        }
    }
}