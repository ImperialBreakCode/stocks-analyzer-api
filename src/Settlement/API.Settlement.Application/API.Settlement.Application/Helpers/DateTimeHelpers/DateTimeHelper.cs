using API.Settlement.Domain.Interfaces.DateTimeInterfaces;

namespace API.Settlement.Application.Helpers.DateTimeHelpers
{
	public class DateTimeHelper : IDateTimeService
	{
		public DateTime UtcNow => DateTime.UtcNow;
		public string GetCronExpressionForEveryFiveMinutes() => "*/5 * * * *";
		public string GetCronExpressionForEveryHour() => "0 * * * *";
		public string GetCronExpressionForEveryTenMinutes() => "*/10 * * * *";
		public TimeSpan GetTimeSpanUntilNextDayAtMinutePastMidnight()
		{
			DateTime currentTime = UtcNow;
			DateTime desiredTime = currentTime.AddDays(1).Date.AddHours(0).AddMinutes(1).AddSeconds(0);
			TimeSpan timeUntilDesiredTime = desiredTime - currentTime;

			return timeUntilDesiredTime;
		}

	}
}