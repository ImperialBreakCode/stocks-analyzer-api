using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.MongoDbServices.WalletDbServices
{
	public class WalletRepository : IWalletRepository
	{
		private readonly IMongoCollection<Wallet> _walletRepository;

		public WalletRepository(IWalletDbSettings walletDbSettings,
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
