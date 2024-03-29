﻿using API.Accounts.Domain.Entities;
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

        public void DeleteByUsername(string userName)
        {
            var command = CreateCommand("DELETE FROM [User] WHERE UserName = @userName");
            command.Parameters.AddWithValue("@userName", userName);
            command.ExecuteNonQuery();
        }

        public User? GetConfirmedByUsername(string username)
        {
            return GetByUsername(username, "SELECT * FROM [User] WHERE UserName = @userName AND IsConfirmed=1");
        }

        public User? GetOneByEmail(string email)
        {
            var command = CreateCommand("SELECT * FROM [User] WHERE [Email] = @email");
            command.Parameters.AddWithValue("@email", email);
            return EntityConverterHelper.ToEntityCollection<User>(command).FirstOrDefault();
        }

        public User? GetOneByUsername(string username)
        {
            return GetByUsername(username, "SELECT * FROM [User] WHERE UserName = @userName");
        }

        public void UpdateByUsername(User user)
        {
            var command = CreateCommand(UpdateQuery.Split("WHERE")[0] + "WHERE UserName = @usernameParam");

            EntityConverterHelper.ToQuery(user, command);

            command.Parameters.AddWithValue("@usernameParam", user.UserName);
            command.ExecuteNonQuery();
        }

        private User? GetByUsername(string username, string query)
        {
            var command = CreateCommand(query);
            command.Parameters.AddWithValue("@userName", username);
            return EntityConverterHelper.ToEntityCollection<User>(command).FirstOrDefault();
        }
    }
}
