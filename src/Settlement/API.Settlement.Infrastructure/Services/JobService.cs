using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using Newtonsoft.Json;

namespace API.Settlement.Infrastructure.Services
{
	public class JobService : IJobService
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public JobService(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public void ProcessNextDayAccountPurchase(BuyStockResponseDTO buyStockResponseDTO)
		{
			//using(var httpClient = _httpClientFactory.CreateClient())
			//{
			//	var json = JsonConvert.SerializeObject(buyStockResponseDTO);
			//	var content = new StringContent(json, Encoding.UTF8, "application/json");
			//	var response = httpClient.PostAsync("api/accounts/finalize...", content);
			//}
			Console.WriteLine("account updated");
		}

		public void ProcessNextDayAccountSale(SellStockResponseDTO sellStockResponseDTO)
		{
			//using(var httpClient = _httpClientFactory.CreateClient())
			//{
			//	var json = JsonConvert.SerializeObject(sellStockResponseDTO);
			//	var content = new StringContent(json, Encoding.UTF8, "application/json");
			//	var response = httpClient.PostAsync("api/accounts/sale...", content);
			//}
			Console.WriteLine("account updated");
		}
	}
}