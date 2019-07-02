using Newtonsoft.Json;

namespace RedSoft.Test.Api.Models
{
	public class TransferResult
	{
		[JsonProperty("source_balance")]
		public decimal SourceBalance { get; set; }

		[JsonProperty("destination_balance")]
		public decimal DestinationBalance { get; set; }

		public TransferResult(decimal sourceBalance, decimal destinationBalance)
		{
			SourceBalance = sourceBalance;
			DestinationBalance = destinationBalance;
		}
	}
}