using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Interfaces;

namespace API.Settlement.Infrastructure.Services
{
	public class UserWalletDictionaryService : IUserWalletDictionaryService
	{
		private readonly IDictionary<string, ICollection<Wallet>> _userWalletDictionary;

		public UserWalletDictionaryService(IDictionary<string, ICollection<Wallet>> userWalletDictionary)
		{
			_userWalletDictionary = userWalletDictionary;
		}

		// TODO: Все още не е готово, логиката не е напълно правилна!
		public decimal CalculateAverageUserWalletPrice(string walletId, string stockId)
		{
			throw new NotImplementedException();
		}

		public void CloseUserWallet(string walletId, string stockId)
		{
			throw new NotImplementedException();
		}

		public void CreateOrUpdateWallet(Wallet wallet)
		{
			throw new NotImplementedException();
		}

	}
}