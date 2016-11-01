﻿using System;

namespace Slalom.Boost.Dynamic.Parsing
{
	internal enum TokenId
	{
		Unknown,
		End,
		Identifier,
		CharLiteral,
		StringLiteral,
		IntegerLiteral,
		RealLiteral,
		Exclamation,
		Percent,
		OpenParen,
		CloseParen,
		Asterisk,
		Plus,
		Comma,
		Minus,
		Dot,
		Slash,
		Colon,
		LessThan,
		GreaterThan,
		Question,
		OpenBracket,
		CloseBracket,
		Bar,
		ExclamationEqual,
		DoubleAmphersand,
		LessThanEqual,
		DoubleEqual,
		GreaterThanEqual,
		DoubleBar,
		Equal
	}

}
