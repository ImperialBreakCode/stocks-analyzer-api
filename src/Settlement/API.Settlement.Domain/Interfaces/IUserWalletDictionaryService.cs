using API.Settlement.Domain.Entities;
using System;
namespace API.Settlement.Domain.Interfaces
{
	public interface IUserWalletDictionaryService
	{
		void CreateOrUpdateWallet(Wallet wallet);
		void CloseUserWallet(string walletId, string stockId);
		decimal CalculateAverageUserWalletPrice(string walletId, string stockId);
	}
}