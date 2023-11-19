using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Interfaces;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace API.Settlement.Infrastructure.Services
{

	public class HangfireService : IHangfireService
	{
		private readonly IDateTimeService _dateTimeService;
		private readonly IJobService _jobService;
		private readonly IInfrastructureConstants _constants;

		public HangfireService(IDateTimeService dateTimeService,
							IJobService jobService,
							IInfrastructureConstants constants)
		{
			_dateTimeService = dateTimeService;
			_jobService = jobService;
			_constants = constants;	
		}


		public void ScheduleStockProcessingJob(AvailabilityResponseDTO availabilityResponseDTO)
			=> BackgroundJob.Schedule(() => _jobService.ProcessNextDayAccountTransaction(availabilityResponseDTO), _dateTimeService.GetTimeSpanUntilNextDayAtMinutePastMidnight());
		public void InitializeRecurringFailedTransactionsJob()
		{
			if(!_constants.IsInitializedRecurringFailedTransactionsJob)
			{
				RecurringJob.AddOrUpdate("recurringFailedTransactionsJob", () => _jobService.RecurringFailedTransactionsJob(), _dateTimeService.GetCronExpressionForEveryHour());
				_constants.IsInitializedRecurringFailedTransactionsJob = true;
			}
		}
		public void InitializeRecurringCapitalLossJobCheck()
		{
			if (!_constants.IsInitializedRecurringCapitalLossCheckJob)
			{
				RecurringJob.AddOrUpdate("recurringCapitalLossJobCheck", () => _jobService.RecurringCapitalLossCheckJob(), _dateTimeService.GetCronExpressionForEveryHour());
				_constants.IsInitializedRecurringCapitalLossCheckJob= true;
			}
		}
		


	}


}