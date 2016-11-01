using System;
using System.CodeDom.Compiler;

namespace Slalom.Boost.Dynamic
{
	[Flags, GeneratedCode("Dynamic", "1")]
	public enum AssignmentOperators
	{
		/// <summary>
		/// Disable all the assignment operators
		/// </summary>
		None = 0,
		/// <summary>
		/// Enable the assignment equal operator
		/// </summary>
		AssignmentEqual = 1,
		/// <summary>
		/// Enable all assignment operators
		/// </summary>
		All = AssignmentEqual
	}
}
