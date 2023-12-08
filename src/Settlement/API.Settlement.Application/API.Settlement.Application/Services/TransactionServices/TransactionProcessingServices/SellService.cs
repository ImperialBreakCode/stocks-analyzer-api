﻿using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Enums;
using API.Settlement.Domain.Interfaces.CommissionInterfaces;
using API.Settlement.Domain.Interfaces.MapperManagementInterfaces;
using API.Settlement.Domain.Interfaces.TransactionInterfaces.OrderProcessingInterfaces;
using Newtonsoft.Json;

namespace API.Settlement.Application.Services.TransactionServices.OrderProcessingServices
{
	public class SellService : ISellService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IMapperManagementWrapper _mapperManagementWrapper;
		private readonly IUserCommissionCalculatorHelper _userCommissionCalculatorHelper;


		public SellService(IHttpClientFactory httpClientFactory,
						   IMapperManagementWrapper mapperManagementWrapper,
						   IUserCommissionCalculatorHelper userCommissionCalculatorHelper)
		{
			_httpClientFactory = httpClientFactory;
			_mapperManagementWrapper = mapperManagementWrapper;
			_userCommissionCalculatorHelper = userCommissionCalculatorHelper;
		}

		public async Task<AvailabilityResponseDTO> SellStocks(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityStockInfoResponseDTOs = await GenerateAvailabilityStockInfoResponseList(finalizeTransactionRequestDTO);

			return MapToAvailabilityResponseDTO(finalizeTransactionRequestDTO, availabilityStockInfoResponseDTOs);
		}

		private async Task<IEnumerable<AvailabilityStockInfoResponseDTO>> GenerateAvailabilityStockInfoResponseList(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO)
		{
			var availabilityStockInfoResponseDTOs = new List<AvailabilityStockInfoResponseDTO>();
			foreach (var stockInfoRequestDTO in finalizeTransactionRequestDTO.StockInfoRequestDTOs)
			{
				//var stockDTO = await GetStockDTO(_infrastructureConstants.GETStockRoute(stockInfoRequestDTO.StockId));
				var stockDTO = new StockDTO { Quantity = 1, StockId = "1", StockName = "mc", WalletId = "1" };

				decimal totalPriceIncludingCommission = CalculateTotalPriceIncludingCommission(stockInfoRequestDTO.TotalPriceExcludingCommission, finalizeTransactionRequestDTO.UserRank);

				var availabilityStockInfoResponseDTO = GenerateAvailabilityStockInfoResponse(stockInfoRequestDTO, stockDTO.Quantity, totalPriceIncludingCommission);
				availabilityStockInfoResponseDTOs.Add(availabilityStockInfoResponseDTO);
			}
			return availabilityStockInfoResponseDTOs;
		}

		private async Task<StockDTO> GetStockDTO(string uri)
		{
			using (var _httpClient = _httpClientFactory.CreateClient())
			{
				var response = await _httpClient.GetAsync(uri);

				if (response.IsSuccessStatusCode)
				{
					var jsonResponse = await response.Content.ReadAsStringAsync();
					var stockDTO = JsonConvert.DeserializeObject<StockDTO>(jsonResponse);
					return stockDTO;
				}
			}
			return null;
		}

		private decimal CalculateTotalPriceIncludingCommission(decimal totalPriceExcludingCommission, UserRank userRank)
		{
			return _userCommissionCalculatorHelper.CalculatePriceAfterAddingSaleCommission(totalPriceExcludingCommission, userRank);
		}

		private AvailabilityStockInfoResponseDTO GenerateAvailabilityStockInfoResponse(StockInfoRequestDTO stockInfoRequestDTO, int availableQuantity, decimal totalPriceIncludingCommission)
		{
			if (availableQuantity < stockInfoRequestDTO.Quantity)
			{
				return _mapperManagementWrapper.AvailabilityStockInfoResponseDTOMapper.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Declined);
			}

			return _mapperManagementWrapper.AvailabilityStockInfoResponseDTOMapper.MapToAvailabilityStockInfoResponseDTO(stockInfoRequestDTO, totalPriceIncludingCommission, Status.Scheduled);
		}

		private AvailabilityResponseDTO MapToAvailabilityResponseDTO(FinalizeTransactionRequestDTO finalizeTransactionRequestDTO, IEnumerable<AvailabilityStockInfoResponseDTO> availabilityStockInfoResponseDTOs)
		{
			return _mapperManagementWrapper.AvailabilityResponseDTOMapper.MapToAvailabilityResponseDTO(finalizeTransactionRequestDTO, availabilityStockInfoResponseDTOs);
		}

	}
}