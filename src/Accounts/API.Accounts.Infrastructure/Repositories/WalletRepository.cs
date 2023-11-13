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

        public ICollection<Wallet> GetDemoWallets()
        {
            var command = CreateCommand($"SELECT * FROM Wallet WHERE IsDemo=1");
            return EntityConverterHelper.ToEntityCollection<Wallet>(command);
        }

        public void DeleteWalletWithItsChildren(string walletId)
        {
            var deleteWalletTransactions = CreateCommand($"DELETE FROM [Transaction] WHERE WalletId=@walletId");
            deleteWalletTransactions.Parameters.AddWithValue("@walletId", walletId);
            deleteWalletTransactions.ExecuteNonQuery();

            var deleteWalletStocks = CreateCommand($"DELETE FROM Stock WHERE WalletId=@walletId");
            deleteWalletStocks.Parameters.AddWithValue("@walletId", walletId);
            deleteWalletStocks.ExecuteNonQuery();

            Delete(walletId);
        }
    }
}
