using System;
using Microsoft.EntityFrameworkCore;
using RedSoft.Test.Api.Models;

namespace RedSoft.Test.Api.Database
{
	public class SqlContext : DbContext
	{
		public DbSet<Account> Accounts { get; set; }
		public DbSet<AccountHistory> AccountHistories { get; set; }
		public SqlContext(DbContextOptions<SqlContext> options) : base(options)
		{ }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Seeding DataBase
			modelBuilder.Entity<Account>().HasData(
				new Account { Id = 1, AccountNumber = "account1", Balance = 3000 },
				new Account { Id = 2, AccountNumber = "account2", Balance = 500 },
				new Account { Id = 3, AccountNumber = "account3", Balance = 6540 },
				new Account { Id = 4, AccountNumber = "account4", Balance = 10000 },
				new Account { Id = 5, AccountNumber = "account5", Balance = 1007 }
			);

			modelBuilder.Entity<AccountHistory>().HasData(
				new AccountHistory { Id = 1, AccountId = 1, Amount = 13640, ChangedAt = new DateTime(2019, 07, 01, 14, 00, 010)},
				new AccountHistory { Id = 2, AccountId = 2, Amount = 610, ChangedAt = new DateTime(2019, 07, 01, 15, 20, 30)},
				new AccountHistory { Id = 3, AccountId = 1, Amount = -10640, ChangedAt = new DateTime(2019, 07, 01, 15, 23, 47)},
				new AccountHistory { Id = 4, AccountId = 4, Amount = 10500, ChangedAt = new DateTime(2019, 07, 01, 15, 59, 30)},
				new AccountHistory { Id = 5, AccountId = 4, Amount = -500, ChangedAt = new DateTime(2019, 07, 01, 18, 07, 21)},
				new AccountHistory { Id = 6, AccountId = 5, Amount = 1007, ChangedAt = new DateTime(2019, 07, 01, 18, 22, 15)},
				new AccountHistory { Id = 7, AccountId = 2, Amount = -20, ChangedAt = new DateTime(2019, 07, 01, 19, 10, 18)},
				new AccountHistory { Id = 8, AccountId = 2, Amount = -90, ChangedAt = new DateTime(2019, 07, 01, 20, 42, 26)},
				new AccountHistory { Id = 9, AccountId = 3, Amount = 6540, ChangedAt = new DateTime(2019, 07, 01, 21, 59, 55)}
			);
		}
	}
}
