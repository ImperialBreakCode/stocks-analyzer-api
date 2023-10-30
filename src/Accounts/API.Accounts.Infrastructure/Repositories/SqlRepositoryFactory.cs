using API.Accounts.Domain.Interfaces;
using Microsoft.Data.SqlClient;

namespace API.Accounts.Infrastructure.Repositories
{
    public class SqlRepositoryFactory : IRepositoryFactory
    {
        private readonly SqlConnection _sqlConnection;
        private readonly SqlTransaction _sqlTransaction;

        public SqlRepositoryFactory(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            _sqlConnection = sqlConnection;
            _sqlTransaction = sqlTransaction;
        }

        public IRepository<T> CreateGenericRepo<T>() where T : IEntity
        {
            return new Repository<T>(_sqlConnection, _sqlTransaction);
        }

        public IUserRepository CreateUserRepo()
        {
            return new UserRepository(_sqlConnection, _sqlTransaction);
        }
    }
}
