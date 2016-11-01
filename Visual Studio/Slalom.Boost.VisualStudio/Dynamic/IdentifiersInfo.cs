﻿using System.Collections.Generic;
using System.Linq;

namespace Slalom.Boost.VisualStudio.Dynamic
{
	public class IdentifiersInfo
	{
		public IdentifiersInfo(
			IEnumerable<string> unknownIdentifiers,
			IEnumerable<Identifier> identifiers,
			IEnumerable<ReferenceType> types)
		{
			this.UnknownIdentifiers = unknownIdentifiers.ToList();
			this.Identifiers = identifiers.ToList();
			this.Types = types.ToList();
		}

		public IEnumerable<string> UnknownIdentifiers { get; private set; }
		public IEnumerable<Identifier> Identifiers { get; private set; }
		public IEnumerable<ReferenceType> Types { get; private set; }
	}
}