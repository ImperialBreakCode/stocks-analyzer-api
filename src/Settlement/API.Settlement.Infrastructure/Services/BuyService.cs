using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using API.Settlement.Infrastructure.Helpers.Enums;

namespace API.Settlement.Infrastructure.Services
{
	public class BuyService : IBuyService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IInfrastructureConstants _infrastructureConstants;
		private readonly ITransactionMapperService _transactionMapperService;
		private readonly IHangfireService _hangfireService;
		private readonly IUserDictionaryService _userDictionaryService;


		public BuyService(IHttpClientFactory httpClientFactory, IInfrastructureConstants constants, ITransactionMapperService buyTransactionMapperService, IHangfireService hangfireService, IUserDictionaryService userDictionaryService)
		{
			_httpClientFactory = httpClientFactory;
			_infrastructureConstants = constants;
			_transactionMapperService = buyTransactionMapperService;
			_hangfireService = hangfireService;
			_userDictionaryService = userDictionaryService;
		}

		public async Task<ICollection<BuyStockResponseDTO>> BuyStocks(ICollection<BuyStockDTO> buyStockDTOs)
		{
			var buyStocksResponseDTOs = new List<BuyStockResponseDTO>();
			foreach (var buyStockDTO in buyStockDTOs)
			{
				decimal accountBalance = await GetAccountBalance(buyStockDTO.UserId);
				decimal totalBuyingPriceIncludingCommission = CalculateTotalBuyingPriceWithCommission(buyStockDTO.TotalBuyingPriceExcludingCommission);

				var buyStockResponseDTO = new BuyStockResponseDTO();
				if (accountBalance < totalBuyingPriceIncludingCommission)
				{
					buyStockResponseDTO = _transactionMapperService.CreateBuyTransactionResponse(buyStockDTO, totalBuyingPriceIncludingCommission, Status.Declined);
				}
				else
				{
					buyStockResponseDTO = _transactionMapperService.CreateBuyTransactionResponse(buyStockDTO, totalBuyingPriceIncludingCommission, Status.Scheduled);

					// TODO: Да добавя логиката за кеширане на новите акции
					// var stockDTO = _transactionMapperService.CreateStockDTO(buyStockResponseDTO, totalBuyingPriceIncludingCommission)
					// _userDictionaryService.AddOrUpdateStock(stockDTO);
				}

				buyStocksResponseDTOs.Add(buyStockResponseDTO);

				_hangfireService.ScheduleBuyStockJob(buyStockResponseDTO);
			}

			return buyStocksResponseDTOs;
		}

		private decimal CalculateTotalBuyingPriceWithCommission(decimal totalBuyingPriceExcludingCommission)
		{
			return totalBuyingPriceExcludingCommission + (totalBuyingPriceExcludingCommission * _infrastructureConstants.Commission);
		}

		private async Task<decimal> GetAccountBalance(string userId)
		{
			decimal balance = 0;
			using (var _httpClient = _httpClientFactory.CreateClient())
			{
				balance = decimal.Parse(await _httpClient.GetStringAsync(_infrastructureConstants.GetAccountBalanceRoute(userId)));
			}
			return balance;
		}
	}
}