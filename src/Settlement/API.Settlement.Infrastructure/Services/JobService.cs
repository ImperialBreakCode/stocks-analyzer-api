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
		private readonly IInfrastructureConstants _InfrastructureConstants;
		private readonly ITransactionWrapper _transactionWrapper;


		public JobService(IHttpClientFactory httpClientFactory, IInfrastructureConstants infrastructureConstants, ITransactionWrapper transactionWrapper)
		{
			_httpClientFactory = httpClientFactory;
			_InfrastructureConstants = infrastructureConstants;
			_transactionWrapper = transactionWrapper;
		}

		public async Task ProcessNextDayAccountTransaction(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var finalizeTransactionResponseDTO = await _transactionWrapper.ProcessNextDayAccountTransaction(finalizeTransactionRequestDTO);
			using (var httpClient = _httpClientFactory.CreateClient())
			{
				var json = JsonConvert.SerializeObject(finalizeTransactionResponseDTO);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				var response = httpClient.PostAsync(_InfrastructureConstants.POSTCompleteTransactionRoute(finalizeTransactionResponseDTO), content);
			}
		}
	}
}