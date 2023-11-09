using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace API.Settlement.Infrastructure.Services
{
	public class JobService : IJobService
	{
		private readonly ITransactionMapperService _transactionMapperService;
		private readonly IHttpClientFactory _httpClientFactory;

		public JobService(IHttpClientFactory httpClientFactory,
						ITransactionMapperService transactionMapperService)
		{
			_httpClientFactory = httpClientFactory;
			_transactionMapperService = transactionMapperService;
		}

		public async Task ProcessNextDayAccountTransaction(AvailabilityResponseDTO availabilityResponseDTO)
		{
			var finalizeTransactionResponseDTO = _transactionMapperService.MapToFinalizeTransactionResponseDTO(availabilityResponseDTO);
			using (var httpClient = _httpClientFactory.CreateClient())
			{
				var json = JsonConvert.SerializeObject(finalizeTransactionResponseDTO);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				//await httpClient.PostAsync(_InfrastructureConstants.POSTCompleteTransactionRoute(finalizeTransactionResponseDTO), content);
				await Console.Out.WriteLineAsync(json);
			}
		}

	}
}