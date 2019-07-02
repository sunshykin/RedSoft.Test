using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RedSoft.Test.Api.Attributes;
using RedSoft.Test.Api.Interfaces;
using RedSoft.Test.Api.Models;
using RedSoft.Test.Api.ViewModel;


namespace RedSoft.Test.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private const string AmountError = "Amount should be decimal and greater than zero";

		private readonly IAccountService accountService;

		public AccountController(IAccountService accountService)
		{
			this.accountService = accountService;
		}


		[HttpGet("{accountId}")]
		public async Task<ActionResult<IResponse>> AccountData(uint accountId)
		{
			return Ok(
				new ResultResponse<Account>(await accountService.GetAccount(accountId))
			);
		}

		[HttpGet("{accountId}/history")]
		public async Task<ActionResult<IResponse>> AccountHistory(uint accountId)
		{
			return Ok(
				new ResultResponse<AccountHistory[]>(await accountService.GetAccountHistory(accountId))
			);
		}

		[HttpPost("{accountId}/top-up")]
		public async Task<ActionResult<IResponse>> TopUp(uint accountId, [FromBody, DecimalGreaterThanZero("Amount")] decimal amount)
		{
			return Ok(
				new ResultResponse<decimal>(await accountService.TopUpFunds(accountId, amount))
			);
		}

		[HttpPost("{accountId}/withdraw")]
		public async Task<ActionResult<IResponse>> Withdraw(uint accountId, [FromBody, DecimalGreaterThanZero("Amount")] decimal amount)
		{
			return Ok(
				new ResultResponse<decimal>(await accountService.WithdrawFunds(accountId, amount))
			);
		}

		[HttpPost("{sourceAccountId}/transfer/{destinationAccountId}")]
		public async Task<ActionResult<IResponse>> Withdraw(
			uint sourceAccountId,
			uint destinationAccountId,
			[FromBody, DecimalGreaterThanZero("Amount")] decimal amount
		)
		{
			return Ok(
				new ResultResponse<TransferResult>(await accountService.TransferFunds(sourceAccountId, destinationAccountId, amount))
			);
		}
	}
}
