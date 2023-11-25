using API.Settlement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces
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
	}
}