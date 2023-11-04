using API.Settlement.Domain.Entities;
using API.Settlement.Domain.Interfaces;

namespace API.Settlement.Infrastructure.Services
{
	public class UserDictionaryService : IUserDictionaryService
	{
		private readonly IDictionary<string, ICollection<Stock>> _userDictionary;

		public UserDictionaryService(IDictionary<string, ICollection<Stock>> userDictionary)
		{
			_userDictionary = userDictionary;
		}

		public void AddOrUpdateStock(string userId, Stock stock)
		{
			if (_userDictionary.ContainsKey(userId))
			{
				ICollection<Stock> stocks = _userDictionary[userId];
				Stock existingStock = stocks.FirstOrDefault(stock);
				if (existingStock != null)
				{
					existingStock.Quantity += stock.Quantity;
					existingStock.InvestedAmount += stock.InvestedAmount;
				}
				else
				{
					stocks.Add(stock);
				}
			}
			else
			{
				List<Stock> stocks = new List<Stock>();
				_userDictionary.Add(userId, stocks);
			}
		}

		public decimal CalculateAveragePrice(string userId, string stockId)
		{
			if (!_userDictionary.ContainsKey(userId)) return 0;

			var userStocks = _userDictionary[userId];
			var selectedStocks = userStocks.Where(x => x.StockId == stockId);

			if (selectedStocks.Count() == 0) return 0;

			decimal totalInvestedAmount = 0;
			int totalQuantity = 0;
			foreach (var currentStock in selectedStocks)
			{
				totalInvestedAmount += currentStock.InvestedAmount;
				totalQuantity += currentStock.Quantity;
			}
			return totalInvestedAmount / totalQuantity;
		}

		public void DeleteStock(string userId, string stockId)
		{
			if (_userDictionary.ContainsKey(userId))
			{
				var stocks = _userDictionary[userId];
				foreach (var currentStock in stocks)
				{
					if (currentStock.StockId == stockId)
					{
						stocks.Remove(currentStock);
					}
				}
			}
		}

	}
}