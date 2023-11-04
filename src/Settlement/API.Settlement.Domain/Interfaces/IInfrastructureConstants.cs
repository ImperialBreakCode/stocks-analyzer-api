namespace API.Settlement.Domain.Interfaces
{
	public interface IInfrastructureConstants
	{
		string TransactionDeclinedMessage { get; }
		string TransactionScheduledMessage { get; }
		string TransactionSuccessMessage { get; }
		decimal Commission { get; }
		string GetAccountBalanceRoute(string userId);
	}
}