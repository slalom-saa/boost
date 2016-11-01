using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Slalom.Boost.VisualStudio.Dynamic.Exceptions
{
	[Serializable]
	public class NoApplicableMethodException : ParseException
	{
		public NoApplicableMethodException(string methodName, string methodTypeName, int position)
			: base(string.Format("No applicable method '{0}' exists in type '{1}'", methodName, methodTypeName), position) 
		{
			this.MethodTypeName = methodTypeName;
			this.MethodName = methodName;
		}

		protected NoApplicableMethodException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context) 
		{
			this.MethodTypeName = info.GetString("MethodTypeName");
			this.MethodName = info.GetString("MethodName");
		}

		public string MethodTypeName { get; private set; }
		public string MethodName { get; private set; }

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("MethodName", this.MethodName);
			info.AddValue("MethodTypeName", this.MethodTypeName);

			base.GetObjectData(info, context);
		}
	}
}
