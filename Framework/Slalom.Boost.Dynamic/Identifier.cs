using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Slalom.Boost.Dynamic
{
    [DebuggerNonUserCode, GeneratedCode("Dynamic", "1")]
	public class Identifier
	{
		public Expression Expression { get; private set; }
		public string Name { get; private set; }

		public Identifier(string name, Expression expression)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			if (expression == null)
				throw new ArgumentNullException("expression");

			this.Expression = expression;
			this.Name = name;
		}
	}
}
