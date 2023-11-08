using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
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
		private readonly IInfrastructureConstants _InfrastructureConstants;

		public JobService(IHttpClientFactory httpClientFactory, 
						ITransactionMapperService transactionMapperService,
						IInfrastructureConstants infrastructureConstants)
		{
			_httpClientFactory = httpClientFactory;
			_transactionMapperService = transactionMapperService;
			_InfrastructureConstants = infrastructureConstants;
		}

		public async Task ProcessNextDayAccountTransaction(AvailabilityResponseDTO availabilityResponseDTO)
		{
			var finalizeTransactionResponseDTO = _transactionMapperService.MapToFinalizeTransactionResponseDTO(availabilityResponseDTO);
			using (var httpClient = _httpClientFactory.CreateClient())
			{
				var json = JsonConvert.SerializeObject(finalizeTransactionResponseDTO);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
                await httpClient.PostAsync(_InfrastructureConstants.POSTCompleteTransactionRoute(finalizeTransactionResponseDTO), content);
            }
		}

	}
}