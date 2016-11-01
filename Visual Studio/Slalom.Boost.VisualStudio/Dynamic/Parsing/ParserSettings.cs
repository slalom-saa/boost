using System;
using System.Collections.Generic;
using System.Reflection;

namespace Slalom.Boost.VisualStudio.Dynamic.Parsing
{
	internal class ParserSettings
	{
		readonly Dictionary<string, Identifier> _identifiers;
		readonly Dictionary<string, ReferenceType> _knownTypes;
		readonly HashSet<MethodInfo> _extensionMethods;

		public ParserSettings(bool caseInsensitive)
		{
			this.CaseInsensitive = caseInsensitive;

			this.KeyComparer = this.CaseInsensitive ? StringComparer.InvariantCultureIgnoreCase : StringComparer.InvariantCulture;

			this.KeyComparison = this.CaseInsensitive ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;

			_identifiers = new Dictionary<string, Identifier>(this.KeyComparer);

			_knownTypes = new Dictionary<string, ReferenceType>(this.KeyComparer);

			_extensionMethods = new HashSet<MethodInfo>();

			this.AssignmentOperators = AssignmentOperators.All;
		}

		public IDictionary<string, ReferenceType> KnownTypes
		{
			get { return _knownTypes; }
		}

		public IDictionary<string, Identifier> Identifiers
		{
			get { return _identifiers; }
		}

		public HashSet<MethodInfo> ExtensionMethods
		{
			get { return _extensionMethods; }
		}

		public bool CaseInsensitive
		{
			get;
			private set;
		}

		public StringComparison KeyComparison
		{
			get;
			private set;
		}

		public IEqualityComparer<string> KeyComparer
		{
			get;
			private set;
		}

		public AssignmentOperators AssignmentOperators
		{
			get;
			set;
		}
	}
}
