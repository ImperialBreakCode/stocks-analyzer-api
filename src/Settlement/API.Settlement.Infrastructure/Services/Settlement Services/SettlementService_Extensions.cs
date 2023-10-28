using API.Settlement.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Infrastructure.Services.Settlement_Services
{
	public partial class SettlementService
	{
		private async Task<decimal> GetAccountBalance(string userId)
		{
			string accountBalanceAsString = await _httpClient.GetStringAsync(APIAccountRoutes.GetAccountBalance(userId));
			decimal accountBalance = decimal.Parse(accountBalanceAsString);
			return accountBalance;
		}
		private TimeSpan GetTimeSpanToNextExecution()
		{
			DateTime currentTime = DateTime.Now;
			DateTime desiredTime = currentTime.AddDays(1).Date.AddHours(0).AddMinutes(1).AddSeconds(0);
			TimeSpan timeUntilDesiredTime = desiredTime - currentTime;
            return timeUntilDesiredTime;
		}
	}
}
