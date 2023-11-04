using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using Hangfire;

namespace API.Settlement.Infrastructure.Services
{
	public class HangfireService : IHangfireService
	{
		private readonly IDateTimeService _dateTimeService;
		private readonly IJobService _jobService;

		public HangfireService(IDateTimeService dateTimeService, IJobService jobService)
		{
			_dateTimeService = dateTimeService;
			_jobService = jobService;
		}

		public void ScheduleBuyStockJob(BuyStockResponseDTO buyStockResponseDTO)
		{
			BackgroundJob.Schedule(() => _jobService.ProcessNextDayAccountPurchase(buyStockResponseDTO), _dateTimeService.GetTimeSpanUntilNextDayAtMinutePastMidnight());
		}

		public void ScheduleSellStockJob(SellStockResponseDTO sellStockResponseDTO)
		{
			BackgroundJob.Schedule(()=> _jobService.ProcessNextDayAccountSale(sellStockResponseDTO), _dateTimeService.GetTimeSpanUntilNextDayAtMinutePastMidnight());
		}
	}
}