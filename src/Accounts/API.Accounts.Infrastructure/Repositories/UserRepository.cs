using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces;
using Microsoft.Data.SqlClient;

namespace API.Accounts.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SqlConnection sqlConnection, SqlTransaction sqlTransaction) : base(sqlConnection, sqlTransaction)
        {
        }

        public void DeleteByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public User GetOneByUserName(string username)
        {
            throw new NotImplementedException();
        }
    }
}
