using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace RedSoft.Test.Api.Models
{
	public class Account
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public uint Id { get; set; }

		[JsonProperty("account_number")]
		public string AccountNumber { get; set; }
		
		public decimal Balance { get; set; }

		[JsonIgnore]
		public List<AccountHistory> AccountHistories { get; set; }

		public Account()
		{
			AccountHistories = new List<AccountHistory>();
		}
	}
}