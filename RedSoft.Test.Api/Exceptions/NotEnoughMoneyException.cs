using System;

namespace RedSoft.Test.Api.Exceptions
{
	public class NotEnoughMoneyException : Exception
	{
		public NotEnoughMoneyException(string message) : base(message) { }
	}
}
