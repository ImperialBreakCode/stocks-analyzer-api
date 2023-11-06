using API.Accounts.Domain.Entities;
using API.Accounts.Domain.Interfaces;
using API.Accounts.Infrastructure.Helpers;
using Microsoft.Data.SqlClient;

namespace API.Accounts.Infrastructure.Repositories
{
    public class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        public WalletRepository(SqlConnection sqlConnection, SqlTransaction sqlTransaction) : base(sqlConnection, sqlTransaction)
        {
        }

        public Wallet? GetUserWallet(string userId)
        {
            var command = CreateCommand($"SELECT * FROM Wallet WHERE UserId=@userId");
            command.Parameters.AddWithValue("@userId", userId);
            return EntityConverterHelper.ToEntityCollection<Wallet>(command).FirstOrDefault();
        }
    }
}
