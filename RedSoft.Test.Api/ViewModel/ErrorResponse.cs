using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RedSoft.Test.Api.Interfaces;

namespace RedSoft.Test.Api.ViewModel
{
	public class ErrorResponse : IResponse
	{
		public string Status { get; set; }
		public string Message { get; set; }

		public ErrorResponse()
		{
			Status = "error";
		}

		public ErrorResponse(string message) : this()
		{
			Message = message;
		}

		public ErrorResponse(object obj) : this()
		{
			Message = JsonConvert.SerializeObject(obj);
		}
	}
}
