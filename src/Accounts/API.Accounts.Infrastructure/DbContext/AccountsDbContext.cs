using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces;
using API.Accounts.Domain.Interfaces.DbContext;
using API.Accounts.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;

namespace API.Accounts.Infrastructure.DbContext
{
    public class AccountsDbContext : IAccountsDbContext
    {
        private SqlConnection _sqlConnection;
        private SqlTransaction _sqlTransaction;

        private IUserRepository _users;
        private IRepository<Stock> _stocks;
        private IRepository<Wallet> _wallets;
        private IRepository<Transaction> _transactions;

        public AccountsDbContext(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
            _sqlConnection.Open();
            _sqlTransaction = _sqlConnection.BeginTransaction();
        }

        public IUserRepository Users 
            => _users ??= new UserRepository(_sqlConnection, _sqlTransaction);

        public IRepository<Stock> Stocks 
            => _stocks ??= new Repository<Stock>(_sqlConnection, _sqlTransaction);

        public IRepository<Wallet> Wallets
            => _wallets ??= new Repository<Wallet>(_sqlConnection, _sqlTransaction);

        public IRepository<Transaction> Transactions 
            => _transactions ??= new Repository<Transaction>(_sqlConnection, _sqlTransaction);

        public void Commit()
        {
            _sqlTransaction.Commit();
        }

        public void Dispose()
        {
            _sqlTransaction?.Dispose();
            _sqlConnection?.Dispose();
        }
    }
}
