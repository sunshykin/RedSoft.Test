﻿using System;

namespace RedSoft.Test.Api.Exceptions
{
	public class NotFoundException : Exception
	{
		public NotFoundException(string message) : base(message) { }
	}
}
