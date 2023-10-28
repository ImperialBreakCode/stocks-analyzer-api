using API.Settlement.Infrastructure.Helpers;

namespace API.Settlement.Infrastructure.Services.SettlementServices
{
	public partial class SettlementService
	{
		private async Task<decimal> GetAccountBalance(string userId)
		{
			string accountBalanceAsString = await _httpClientService.GetStringAsync(APIAccountRoutes.GetAccountBalance(userId));
			decimal accountBalance = decimal.Parse(accountBalanceAsString);
			return accountBalance;
		}
		private TimeSpan GetTimeSpanToNextExecution()
		{
			DateTime currentTime = _dateTimeService.UtcNow;
			DateTime desiredTime = currentTime.AddDays(1).Date.AddHours(0).AddMinutes(1).AddSeconds(0);
			TimeSpan timeUntilDesiredTime = desiredTime - currentTime;

			return timeUntilDesiredTime;
		}
	}
}