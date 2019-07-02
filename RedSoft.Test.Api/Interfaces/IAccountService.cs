using System.Threading.Tasks;
using RedSoft.Test.Api.Models;

namespace RedSoft.Test.Api.Interfaces
{
	public interface IAccountService
	{
		Task<Account> GetAccount(uint accountId);
		Task<AccountHistory[]> GetAccountHistory(uint accountId);
		Task<decimal> TopUpFunds(uint accountId, decimal amount);
		Task<decimal> WithdrawFunds(uint accountId, decimal amount);
		Task<TransferResult> TransferFunds(uint sourceAccountId, uint destinationAccountId, decimal amount);
	}
}
