using API.Settlement.Domain.Entities.MongoDatabaseEntities.WalletDatabaseEntities;
using API.Settlement.Domain.Interfaces.DatabaseInterfaces.MongoDatabaseInterfaces.WalletDatabaseInterfaces;
using MongoDB.Driver;

namespace API.Settlement.Infrastructure.MongoDbServices.WalletDatabaseServices
{
	public class WalletRepository : IWalletRepository
    {
        private readonly IMongoCollection<Wallet> _walletRepository;

        public WalletRepository(IWalletDatabaseSettings walletDbSettings,
                            IMongoClient mongoClient)
        {
            var walletDatabase = mongoClient.GetDatabase(walletDbSettings.DatabaseName);
            _walletRepository = walletDatabase.GetCollection<Wallet>(walletDbSettings.WalletsCollectionName);
        }
        public void CreateWallet(Wallet wallet)
        {
            _walletRepository.InsertOne(wallet);
        }

        public void DeleteWallet(string id)
        {
            _walletRepository.DeleteOne(wallet => wallet.WalletId == id);
        }

        public Wallet? GetWalletById(string id)
        {
            return _walletRepository.Find(wallet => wallet.WalletId == id).FirstOrDefault();
        }

        public IEnumerable<Wallet> GetWallets()
        {
            return _walletRepository.Find(wallet => true).ToEnumerable();
        }

        public void UpdateWallet(string id, Wallet wallet)
        {
            _walletRepository.ReplaceOne(wallet => wallet.WalletId == id, wallet);
        }
        public Stock? GetStockFromWallet(string walletId, string stockId)
        {
            var filter = Builders<Wallet>.Filter.And(
                Builders<Wallet>.Filter.Eq(w => w.WalletId, walletId),
                Builders<Wallet>.Filter.ElemMatch(w => w.Stocks, s => s.StockId == stockId)
                );

            var wallet = _walletRepository.Find(filter).SingleOrDefault();
            if (wallet == null) { return null; }
            return wallet.Stocks.FirstOrDefault(s => s.StockId == stockId);
        }
        public void AddStock(string walletId, Stock stock)
        {
            var filter = Builders<Wallet>.Filter.Eq(w => w.WalletId, walletId);
            var update = Builders<Wallet>.Update.Push(w => w.Stocks, stock);
            _walletRepository.UpdateOne(filter, update);
        }
        public void RemoveStock(string walletId, string stockId)
        {
            var filter = Builders<Wallet>.Filter.Eq(w => w.WalletId, walletId);
            var update = Builders<Wallet>.Update.PullFilter(w => w.Stocks, s => s.StockId == stockId);
            _walletRepository.UpdateOne(filter, update);
        }

        public void UpdateStock(string walletId, Stock? updatedStock)
        {
            var filter = Builders<Wallet>.Filter.And(
                Builders<Wallet>.Filter.Eq(w => w.WalletId, walletId),
                Builders<Wallet>.Filter.ElemMatch(w => w.Stocks, s => s.StockId == updatedStock.StockId)
                );

            var update = Builders<Wallet>.Update.Set("Stocks.$", updatedStock);

            _walletRepository.UpdateOne(filter, update);
        }
    }
}
