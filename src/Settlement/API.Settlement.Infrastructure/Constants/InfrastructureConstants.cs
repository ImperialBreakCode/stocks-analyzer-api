namespace API.Settlement.Infrastructure.Constants
{
	public static class InfrastructureConstants
	{
		public const string TransactionDeclinedMessage = "Transaction declined.";
		public const string TransactionScheduledMessage = "Transaction scheduled for execution at 00:01:00 next day.";
		public const string TransactionSuccessMessage = "Transaction accepted.";

		public const decimal Commission = 0.0005M;
	}
}