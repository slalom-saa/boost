using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Slalom.Boost.Dynamic.Exceptions
{
	[Serializable, GeneratedCode("Dynamic", "1"), DebuggerNonUserCode]
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
