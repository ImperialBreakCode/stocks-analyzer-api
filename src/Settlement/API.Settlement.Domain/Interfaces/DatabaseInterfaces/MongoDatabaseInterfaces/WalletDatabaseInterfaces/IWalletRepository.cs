using API.Settlement.Domain.Entities.MongoDatabaseEntities.WalletDatabaseEntities;

namespace API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces
{
	public interface IWalletRepository
    {
        IEnumerable<Wallet> GetWallets();
        Wallet? GetWalletById(string id);
        void CreateWallet(Wallet wallet);
        void UpdateWallet(string id, Wallet wallet);
        void DeleteWallet(string id);
        Stock? GetStockFromWallet(string walletId, string stockId);
        void AddStock(string walletId, Stock stock);
        void RemoveStock(string walletId, string stockId);
        void UpdateStock(string walletId, Stock? existingStock);
		bool ContainsWallet(string walletId);
	}
}