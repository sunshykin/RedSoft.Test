using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RedSoft.Test.Api.Database;
using RedSoft.Test.Api.Exceptions;
using RedSoft.Test.Api.Interfaces;
using RedSoft.Test.Api.Models;

namespace RedSoft.Test.Api.Implementations
{
	public class AccountService : IAccountService
	{
		private readonly SqlContext context;

		public AccountService(SqlContext context)
		{
			this.context = context;
		}

		private Task<bool> IsAccountExist(uint id) => context.Accounts.AnyAsync(a => a.Id.Equals(id));

		private async Task<uint> GetNewHistoryId() => await context.AccountHistories.MaxAsync(ah => ah.Id) + 1;

		public async Task<Account> GetAccount(uint accountId)
		{
			if (!await IsAccountExist(accountId))
				throw new NotFoundException($"Account with id '{accountId}' not found.");

			return await context.Accounts.FindAsync(accountId);
		}

		public async Task<AccountHistory[]> GetAccountHistory(uint accountId)
		{
			if (!await IsAccountExist(accountId))
				throw new NotFoundException($"Account with id '{accountId}' not found.");

			var account = await context.Accounts.FindAsync(accountId);

			await context.Entry(account)
				.Collection(a => a.AccountHistories)
				.LoadAsync();

			return account.AccountHistories.ToArray();
		}

		public async Task<decimal> TopUpFunds(uint accountId, decimal amount)
		{
			if (!await IsAccountExist(accountId))
				throw new NotFoundException($"Account with id '{accountId}' not found.");

			var account = await context.Accounts.FindAsync(accountId);
			account.Balance += amount;

			var historyId = await GetNewHistoryId();
			await context.AccountHistories.AddAsync(new AccountHistory(historyId, accountId, amount));

			await context.SaveChangesAsync();

			return account.Balance;
		}

		public async Task<decimal> WithdrawFunds(uint accountId, decimal amount)
		{
			if (!await IsAccountExist(accountId))
				throw new NotFoundException($"Account with id '{accountId}' not found.");

			var account = await context.Accounts.FindAsync(accountId);

			if (account.Balance < amount)
				throw new NotEnoughMoneyException("Not enough money on your balance to withdraw");

			account.Balance -= amount;

			var historyId = await GetNewHistoryId();
			await context.AccountHistories.AddAsync(new AccountHistory(historyId, accountId, -amount));

			await context.SaveChangesAsync();

			return account.Balance;
		}

		public async Task<TransferResult> TransferFunds(uint sourceAccountId, uint destinationAccountId, decimal amount)
		{
			if (!await IsAccountExist(sourceAccountId))
				throw new NotFoundException($"Account with id '{sourceAccountId}' not found.");
			if (!await IsAccountExist(destinationAccountId))
				throw new NotFoundException($"Account with id '{destinationAccountId}' not found.");

			var sourceAccount = await context.Accounts.FindAsync(sourceAccountId);
			var destinationAccount = await context.Accounts.FindAsync(destinationAccountId);

			if (sourceAccount.Balance < amount)
				throw new NotEnoughMoneyException("Not enough money on your balance to transfer");

			sourceAccount.Balance -= amount;
			destinationAccount.Balance += amount;

			var newId = await GetNewHistoryId();

			await context.AccountHistories.AddRangeAsync(
				new AccountHistory(newId, sourceAccountId, -amount),
				new AccountHistory(newId + 1, destinationAccountId, amount)
			);

			// EF's SaveChanges work as a transaction in some way, so not needed to add one here
			await context.SaveChangesAsync();

			return new TransferResult(sourceAccount.Balance, destinationAccount.Balance);
		}
	}
}
