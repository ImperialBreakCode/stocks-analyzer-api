namespace API.Settlement.Domain.Interfaces.DateTimeInterfaces
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }
        TimeSpan GetTimeSpanUntilNextDayAtMinutePastMidnight();
        string GetCronExpressionForEveryFiveMinutes();
        string GetCronExpressionForEveryHour();
        string GetCronExpressionForEveryTenMinutes();

	}
}