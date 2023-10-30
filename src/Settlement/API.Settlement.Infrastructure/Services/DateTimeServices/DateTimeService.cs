using API.Settlement.Domain.Interfaces;

namespace API.Settlement.Infrastructure.Services.DateTimeServices
{
	public class DateTimeService : IDateTimeService
	{
		public DateTime UtcNow
		{
			get
			{
				return DateTime.UtcNow;
			}
		}

	}
}
//TODO: Трябва да се промени, защото давам достъп до всички полета и методи на обекта DateTime!