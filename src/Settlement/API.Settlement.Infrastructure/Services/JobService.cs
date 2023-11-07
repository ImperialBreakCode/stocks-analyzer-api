using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Constants;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace API.Settlement.Infrastructure.Services
{
	public class JobService : IJobService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ITransactionMapperService _transactionMapperService;

		public JobService(IHttpClientFactory httpClientFactory, 
						ITransactionMapperService transactionMapperService)
		{
			_httpClientFactory = httpClientFactory;
			_transactionMapperService = transactionMapperService;
		}

		public async Task ProcessNextDayAccountTransaction(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			var transactionSuccessfulStocks = _transactionMapperService.FilterTransactionSuccessfulStocks(finalizeTransactionResponseDTO);
			using (var httpClient = _httpClientFactory.CreateClient())
			{
				var json = JsonConvert.SerializeObject(transactionSuccessfulStocks);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
                //await httpClient.PostAsync(_InfrastructureConstants.POSTCompleteTransactionRoute(filteredfinalizeTransactionResponseDTO), content);
                await Console.Out.WriteLineAsync(json);
            }
		}

	}
}