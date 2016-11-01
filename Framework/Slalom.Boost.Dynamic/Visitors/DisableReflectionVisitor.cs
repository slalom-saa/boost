using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Slalom.Boost.Dynamic.Exceptions;

namespace Slalom.Boost.Dynamic.Visitors
{
    [DebuggerNonUserCode, GeneratedCode("Dynamic", "1")]
    public class DisableReflectionVisitor : ExpressionVisitor
	{
		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			if (node.Object != null
				&& (typeof(Type).IsAssignableFrom(node.Object.Type)
				|| typeof(MemberInfo).IsAssignableFrom(node.Object.Type)))
			{
				throw new ReflectionNotAllowedException();
			}

			return base.VisitMethodCall(node);
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			if ((typeof(Type).IsAssignableFrom(node.Member.DeclaringType)
				|| typeof(MemberInfo).IsAssignableFrom(node.Member.DeclaringType))
				&& node.Member.Name != "Name")
			{
				throw new ReflectionNotAllowedException();
			}

			return base.VisitMember(node);
		}
	}
}
