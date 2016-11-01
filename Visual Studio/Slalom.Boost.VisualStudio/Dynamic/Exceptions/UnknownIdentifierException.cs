using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Slalom.Boost.VisualStudio.Dynamic.Exceptions
{
	[Serializable]
	public class UnknownIdentifierException : ParseException
	{
		public UnknownIdentifierException(string identifier, int position)
			: base(string.Format("Unknown identifier '{0}'", identifier), position) 
		{
			this.Identifier = identifier;
		}

		protected UnknownIdentifierException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context) 
		{
			this.Identifier = info.GetString("Identifier");
		}

		public string Identifier { get; private set; }

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Identifier", this.Identifier);

			base.GetObjectData(info, context);
		}
	}
}
