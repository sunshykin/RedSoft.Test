using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace RedSoft.Test.Api.Models
{
	public class AccountHistory
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public uint Id { get; set; }

		[JsonProperty("account_id")]
		public uint AccountId { get; set; }

		public decimal Amount { get; set; }

		[JsonProperty("changed_at")]
		public DateTime ChangedAt { get; set; }

		public AccountHistory() { }
		public AccountHistory(uint id, uint accountId, decimal amount)
		{
			Id = id;
			AccountId = accountId;
			Amount = amount;
			ChangedAt = DateTime.Now;
		}
	}
}
