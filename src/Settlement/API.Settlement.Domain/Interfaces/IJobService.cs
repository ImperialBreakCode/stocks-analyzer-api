using API.Settlement.Domain.DTOs.Response;

namespace API.Settlement.Domain.Interfaces
{
	public interface IJobService
	{
		void ProcessNextDayAccountTransactions(IEnumerable<ResponseStockDTO> responseStockDTOs);
	}
}