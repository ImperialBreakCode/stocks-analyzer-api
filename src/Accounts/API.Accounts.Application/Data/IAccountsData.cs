﻿using API.Accounts.Domain.Interfaces.DbContext;

namespace API.Accounts.Application.Data
{
    public interface IAccountsData
    {
        public void EnsureDatabase();
        public void SeedData();
        public IAccountsDbContext CreateDbContext();
    }
}
