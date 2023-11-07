using API.Settlement.Domain.DTOs.Request;
using API.Settlement.Domain.DTOs.Response;
using API.Settlement.Domain.Interfaces;
using Hangfire;

namespace API.Settlement.Infrastructure.Services
{
	public class HangfireService : IHangfireService
	{
		private readonly IDateTimeService _dateTimeService;
		private readonly IJobService _jobService;

		public HangfireService(IDateTimeService dateTimeService, 
							IJobService jobService)
		{
			_dateTimeService = dateTimeService;
			_jobService = jobService;
		}

		public void ScheduleStockProcessingJob(FinalizeTransactionResponseDTO finalizeTransactionResponseDTO)
		{
			BackgroundJob.Schedule(() => _jobService.ProcessNextDayAccountTransaction(finalizeTransactionResponseDTO), _dateTimeService.GetTimeSpanUntilNextDayAtMinutePastMidnight());
		}

	}
}