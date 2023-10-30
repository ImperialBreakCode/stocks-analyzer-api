using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces;
using API.Accounts.Infrastructure.Helpers;
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
            var command = CreateCommand($"DELETE FROM [{typeof(User).Name}] WHERE UserName = @userName");
            command.Parameters.AddWithValue("@userName", userName);
            command.ExecuteNonQuery();
        }

        public User? GetOneByUserName(string username)
        {
            var command = CreateCommand($"SELECT * FROM [{typeof(User).Name}] WHERE UserName = @userName");
            command.Parameters.AddWithValue("@userName", username);
            return EntityConverterHelper.ToEntityCollection<User>(command).FirstOrDefault();
        }
    }
}
