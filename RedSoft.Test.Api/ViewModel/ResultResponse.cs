using RedSoft.Test.Api.Interfaces;

namespace RedSoft.Test.Api.ViewModel
{
	public class ResultResponse<T> : IResponse
	{
		public string Status { get; set; }
		public T Result { get; set; }

		public ResultResponse(T result)
		{
			Status = "ok";
			Result = result;
		}
	}
}
