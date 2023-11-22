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

        protected override string GetByIdQuery => base.GetByIdQuery + " && IsConfirmed=true";
        protected override string GetAllQuery => base.GetAllQuery + " WHERE IsConfirmed=true";

        public void DeleteByUserName(string userName)
        {
            var command = CreateCommand($"DELETE FROM [User] WHERE UserName = @userName");
            command.Parameters.AddWithValue("@userName", userName);
            command.ExecuteNonQuery();
        }

        public User? GetOneByEmail(string email)
        {
            var command = CreateCommand($"SELECT * FROM [User] WHERE [Email] = @email && IsConfirmed=true");
            command.Parameters.AddWithValue("@email", email);
            return EntityConverterHelper.ToEntityCollection<User>(command).FirstOrDefault();
        }

        public User? GetOneByUserName(string username)
        {
            var command = CreateCommand($"SELECT * FROM [User] WHERE UserName = @userName && IsConfirmed=true");
            command.Parameters.AddWithValue("@userName", username);
            return EntityConverterHelper.ToEntityCollection<User>(command).FirstOrDefault();
        }
    }
}
