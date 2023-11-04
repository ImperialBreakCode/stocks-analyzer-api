using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Entities;
using API.Settlement.Infrastructure.Helpers.Enums;

namespace API.Settlement.Domain.Interfaces
{
	public interface ITransactionMapperService
	{
		ResponseStockDTO CreateTransactionResponse(RequestStockDTO requestStockDTO, decimal totalPriceIncludingCommission, Status status);
		Wallet CreateStockDTO(RequestStockDTO requestStockDTO);
	}
}