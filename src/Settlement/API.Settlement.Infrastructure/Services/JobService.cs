using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace API.Settlement.Infrastructure.Services
{
	public class JobService : IJobService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IInfrastructureConstants _InfrastructureConstants;
		public JobService(IHttpClientFactory httpClientFactory, IInfrastructureConstants infrastructureConstants)
		{
			_httpClientFactory = httpClientFactory;
			_InfrastructureConstants = infrastructureConstants;
		}

		public void ProcessNextDayAccountTransactions(IEnumerable<ResponseStockDTO> responseStockDTOs)
		{
			using (var httpClient = _httpClientFactory.CreateClient())
			{
				var json = JsonConvert.SerializeObject(responseStockDTOs);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				var response = httpClient.PostAsync(_InfrastructureConstants.GetFinalizeStocksRoute(responseStockDTOs), content);
			}
		}


		//public void ProcessNextDayAccountPurchase(ResponseStockDTO buyStockResponseDTO)
		//{
		//	//using(var httpClient = _httpClientFactory.CreateClient())
		//	//{
		//	//	var json = JsonConvert.SerializeObject(buyStockResponseDTO);
		//	//	var content = new StringContent(json, Encoding.UTF8, "application/json");
		//	//	var response = httpClient.PostAsync("api/accounts/finalize...", content);
		//	//}
		//	Console.WriteLine("account updated");
		//}

		//public void ProcessNextDayAccountSale(SellStockResponseDTO sellStockResponseDTO)
		//{
		//	//using(var httpClient = _httpClientFactory.CreateClient())
		//	//{
		//	//	var json = JsonConvert.SerializeObject(sellStockResponseDTO);
		//	//	var content = new StringContent(json, Encoding.UTF8, "application/json");
		//	//	var response = httpClient.PostAsync("api/accounts/sale...", content);
		//	//}
		//	Console.WriteLine("account updated");
		//}
	}
}