using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;

namespace API.Settlement.Domain.Interfaces
{
	public interface IJobService
	{
		Task ProcessNextDayAccountTransactions(IEnumerable<FinalizeTransactionRequestDTO> finalizeTransactionRequestDTOs);
	}
}