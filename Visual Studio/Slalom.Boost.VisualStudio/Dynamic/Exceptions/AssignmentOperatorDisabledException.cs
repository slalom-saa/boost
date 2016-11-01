using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Slalom.Boost.VisualStudio.Dynamic.Exceptions
{
	[Serializable]
	public class AssignmentOperatorDisabledException : ParseException
	{
		public AssignmentOperatorDisabledException(string operatorString, int position)
			: base(string.Format("Assignment operator '{0}' not allowed", operatorString), position) 
		{
			this.OperatorString = operatorString;
		}

		protected AssignmentOperatorDisabledException(
			SerializationInfo info,
			StreamingContext context)
			: base(info, context) 
		{
			this.OperatorString = info.GetString("OperatorString");
		}

		public string OperatorString { get; private set; }

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("OperatorString", this.OperatorString);

			base.GetObjectData(info, context);
		}
	}
}
