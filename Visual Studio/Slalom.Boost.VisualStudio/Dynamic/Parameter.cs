using System;
using System.Linq.Expressions;

namespace Slalom.Boost.VisualStudio.Dynamic
{
	/// <summary>
	/// An expression parameter. This class is thread safe.
	/// </summary>
	public class Parameter
	{
		public Parameter(string name, object value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			this.Name = name;
			this.Type = value.GetType();
			this.Value = value;

			this.Expression = System.Linq.Expressions.Expression.Parameter(this.Type, name);
		}

		public Parameter(string name, Type type, object value = null)
		{
			this.Name = name;
			this.Type = type;
			this.Value = value;

			this.Expression = System.Linq.Expressions.Expression.Parameter(type, name);
		}

		public string Name { get; private set; }
		public Type Type { get; private set; }
		public object Value { get; private set; }

		public ParameterExpression Expression { get; private set; }
	}
}
