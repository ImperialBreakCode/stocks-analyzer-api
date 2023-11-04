using API.Settlement.Domain.DTOs.Response;

namespace API.Settlement.Domain.Interfaces
{
	public interface IInfrastructureConstants
	{
		string TransactionDeclinedMessage { get; }
		string TransactionScheduledMessage { get; }
		string TransactionSuccessMessage { get; }
		decimal Commission { get; }
		string GetWalletBalanceRoute(string walletId);
		string GetFinalizeStocksRoute(IEnumerable<ResponseStockDTO> responseStockDTOs);
	}
}