using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

		public Wallet GetWalletById(string id)
		{
			return  _walletRepository.Find(wallet => wallet.WalletId == id).FirstOrDefault();
		}

		public IEnumerable<Wallet> GetWallets()
		{
			return _walletRepository.Find(wallet => true).ToEnumerable();
		}

		public void UpdateWallet(string id, Wallet wallet)
		{
			_walletRepository.ReplaceOne(wallet => wallet.WalletId == id, wallet);
		}
	}
}
