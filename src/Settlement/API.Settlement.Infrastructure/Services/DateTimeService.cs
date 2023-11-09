using API.Settlement.Domain.Interfaces;

namespace API.Settlement.Infrastructure.Services
{
	public class DateTimeService : IDateTimeService
	{

		public DateTime UtcNow => DateTime.UtcNow;

		public string GetCronExpressionForEveryFiveMinutes() => "*/5 * * * *";

		public TimeSpan GetTimeSpanUntilNextDayAtMinutePastMidnight()
		{
			DateTime currentTime = UtcNow;
			DateTime desiredTime = currentTime.AddDays(1).Date.AddHours(0).AddMinutes(1).AddSeconds(0);
			TimeSpan timeUntilDesiredTime = desiredTime - currentTime;

			return timeUntilDesiredTime;
		}

	}
}