﻿using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Slalom.Boost.VisualStudio.Dynamic.Exceptions
{
	[Serializable]
	public class ParseException : DynamicExpressoException
	{
		const string PARSE_EXCEPTION_FORMAT = "{0} (at index {1}).";

		public ParseException(string message, int position)
			: base(string.Format(PARSE_EXCEPTION_FORMAT, message, position)) 
		{
			this.Position = position;
		}

		protected ParseException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context) 
		{
			this.Position = info.GetInt32("Position");
		}

		public int Position { get; private set; }

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Position", this.Position);

			base.GetObjectData(info, context);
		}
	}
}
