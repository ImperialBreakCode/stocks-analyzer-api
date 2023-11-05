using API.Settlement.Domain.DTOs.Response;

namespace API.Settlement.Domain.Interfaces
{
	public interface IInfrastructureConstants
	{
		string TransactionDeclinedMessage { get; }
		string TransactionScheduledMessage { get; }
		string TransactionSuccessMessage { get; }
		decimal Commission { get; }
		string GETWalletBalanceRoute(string walletId);
		string POSTCompleteTransactionRoute(IEnumerable<FinalizeTransactionResponseDTO> responseStockDTOs);
		string GETStockRoute(string stockId);
	}
}