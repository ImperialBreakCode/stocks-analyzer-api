using API.Settlement.Domain.Entities;
using System;
namespace API.Settlement.Domain.Interfaces
{
	public interface IUserDictionaryService
	{
		void AddOrUpdateStock(string userId, Stock stock);
		void DeleteStock(string userId, string stockId);
		decimal CalculateAveragePrice(string userId, string stockId);
	}
}