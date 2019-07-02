using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RedSoft.Test.Api.Exceptions;
using RedSoft.Test.Api.ViewModel;

namespace RedSoft.Test.Api.Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate next;

		public ExceptionHandlingMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
				throw;
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			HttpStatusCode code;

			switch (exception)
			{
				case NotFoundException _:
					code = HttpStatusCode.NotFound;
					break;
				case NotEnoughMoneyException _:
					code = HttpStatusCode.BadRequest;
					break;
				default:
					code = HttpStatusCode.InternalServerError;
					break;
			}

			var result = JsonConvert.SerializeObject(
				new ErrorResponse(exception.Message),
				new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
			);
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)code;

			return context.Response.WriteAsync(result);
		}
	}
}
