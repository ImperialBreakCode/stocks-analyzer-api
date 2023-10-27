using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces;
using API.Accounts.Domain.Interfaces.DbContext;
using API.Accounts.Infrastructure.Repositories;
using Microsoft.Data.SqlClient;

namespace API.Accounts.Infrastructure.DbContext
{
    public class AccountsDbContext : IAccountsDbContext
    {
        private readonly SqlConnection _sqlConnection;
        private readonly SqlTransaction _sqlTransaction;

        private readonly IRepositoryFactory _repositoryFactory;

        private IUserRepository _users;
        private IRepository<Stock> _stocks;
        private IRepository<Wallet> _wallets;
        private IRepository<Transaction> _transactions;

        public AccountsDbContext(string connectionString)
        {
            _sqlConnection = new SqlConnection(connectionString);
            _sqlConnection.Open();
            _sqlTransaction = _sqlConnection.BeginTransaction();

            _repositoryFactory = new SqlRepositoryFactory(_sqlConnection, _sqlTransaction);
        }

        public IUserRepository Users 
            => _users ??= _repositoryFactory.CreateUserRepo();

        public IRepository<Stock> Stocks 
            => _stocks ??= _repositoryFactory.CreateGenericRepo<Stock>();

        public IRepository<Wallet> Wallets
            => _wallets ??= _repositoryFactory.CreateGenericRepo<Wallet>();

        public IRepository<Transaction> Transactions 
            => _transactions ??= _repositoryFactory.CreateGenericRepo<Transaction>();

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
