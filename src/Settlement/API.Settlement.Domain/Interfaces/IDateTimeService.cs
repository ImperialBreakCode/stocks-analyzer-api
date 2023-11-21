namespace API.Settlement.Domain.Interfaces
{
	public interface IDateTimeService
	{
		DateTime UtcNow { get; }
		TimeSpan GetTimeSpanUntilNextDayAtMinutePastMidnight();
		string GetCronExpressionForEveryFiveMinutes();
		string GetCronExpressionForEveryHour();
	}
}