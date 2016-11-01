﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Slalom.Boost.VisualStudio.Dynamic.Reflection;

namespace Slalom.Boost.VisualStudio.Dynamic
{
	public class ReferenceType
	{
		public Type Type { get; private set; }

		/// <summary>
		/// Public name that must be used in the expression.
		/// </summary>
		public string Name { get; private set; }

		public IList<MethodInfo> ExtensionMethods { get; private set; }

		public ReferenceType(string name, Type type)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");

			if (type == null)
				throw new ArgumentNullException("type");

			this.Type = type;
			this.Name = name;
			this.ExtensionMethods = ReflectionExtensions.GetExtensionMethods(type).ToList();
		}

		public ReferenceType(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			this.Type = type;
			this.Name = type.Name;
			this.ExtensionMethods = ReflectionExtensions.GetExtensionMethods(type).ToList();
		}
	}
}
