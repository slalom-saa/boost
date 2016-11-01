using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Slalom.Boost.Validation
{
    /// <summary>
    /// Contains information about a validation message.
    /// </summary>
    [Serializable]
    public class ValidationMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
        /// </summary>
        public ValidationMessage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
        /// </summary>
        /// <param name="message">The message text.</param>
        public ValidationMessage(string message)
            : this(null, message, ValidationMessageType.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
        /// </summary>
        /// <param name="message">The message text.</param>
        /// <param name="type">The validation type.</param>
        public ValidationMessage(string message, ValidationMessageType type)
            : this(null, message, type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
        /// </summary>
        /// <param name="code">The message code for client consumption.</param>
        /// <param name="message">The message text.</param>
        public ValidationMessage(string code, string message)
            : this(code, message, ValidationMessageType.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
        /// </summary>
        /// <param name="code">The message code for client consumption.</param>
        /// <param name="message">The message text.</param>
        /// <param name="type">The validation type.</param>
        public ValidationMessage(string code, string message, ValidationMessageType type)
            : this(code, message, null, type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
        /// </summary>
        /// <param name="code">The message code for client consumption.</param>
        /// <param name="message">The message text.</param>
        /// <param name="helpUrl">The help URL.</param>
        public ValidationMessage(string code, string message, string helpUrl)
            : this(code, message, helpUrl, ValidationMessageType.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationMessage"/> class.
        /// </summary>
        /// <param name="code">The message code for client consumption.</param>
        /// <param name="message">The message text.</param>
        /// <param name="helpUrl">The help URL.</param>
        /// <param name="type">The validation type.</param>
        public ValidationMessage(string code, string message, string helpUrl, ValidationMessageType type)
        {
            this.Code = code;
            this.Message = message;
            this.HelpUrl = helpUrl;
            this.MessageType = type;
        }

        /// <summary>
        /// Gets the message code for client consumption.
        /// </summary>
        /// <value>
        /// The message code.
        /// </value>
        public string Code { get; private set; }

        /// <summary>
        /// Gets the message text.
        /// </summary>
        /// <value>
        /// The message text.
        /// </value>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the help URL.
        /// </summary>
        /// <value>
        /// The help URL.
        /// </value>
        public string HelpUrl { get; private set; }

        /// <summary>
        /// Gets validation type.
        /// </summary>
        /// <value>
        /// The validation type.
        /// </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public ValidationMessageType MessageType { get; private set; }

        /// <summary>
        /// Adds the type to the message.
        /// </summary>
        /// <param name="type">The type of validation.</param>
        /// <returns>The current instance.</returns>
        public ValidationMessage WithType(ValidationMessageType type)
        {
            this.MessageType = type;
            return this;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="ValidationMessage"/>.
        /// </summary>
        /// <param name="message">The message text.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ValidationMessage(string message)
        {
            return new ValidationMessage(message);
        }
    }
}