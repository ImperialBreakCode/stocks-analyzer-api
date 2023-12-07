using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
using API.Settlement.Domain.Interfaces.HangfireInterfaces;
using API.Settlement.Domain.Interfaces.HelpersInterfaces;
using Hangfire;

namespace API.Settlement.Application.Services.HangfireServices
{

	public class HangfireService : IHangfireService
	{
		private readonly IDateTimeService _dateTimeService;
		private readonly IHangfireJobService _jobService;
		private readonly IInfrastructureConstants _constants;

		public HangfireService(IDateTimeService dateTimeService,
							   IHangfireJobService jobService,
							   IInfrastructureConstants constants)
		{
			_dateTimeService = dateTimeService;
			_jobService = jobService;
			_constants = constants;
		}

		public void ScheduleStockProcessingJob(AvailabilityResponseDTO availabilityResponseDTO)
		{
			BackgroundJob.Schedule(() => _jobService.ProcessNextDayAccountTransaction(availabilityResponseDTO), _dateTimeService.GetTimeSpanUntilNextDayAtMinutePastMidnight());
		}

		public void InitializeRecurringFailedTransactionsJob()
		{
			if (!_constants.IsInitializedRecurringFailedTransactionsJob)
			{
				RecurringJob.AddOrUpdate("recurringFailedTransactionsJob", () => _jobService.RecurringFailedTransactionsJob(), _dateTimeService.GetCronExpressionForEveryHour());
				_constants.IsInitializedRecurringFailedTransactionsJob = true;
			}
		}

		public void InitializeRecurringCapitalLossJobCheck()
		{
			if (!_constants.IsInitializedRecurringCapitalLossCheckJob)
			{
				RecurringJob.AddOrUpdate("recurringCapitalLossJobCheck", () => _jobService.RecurringCapitalCheckJob(), _dateTimeService.GetCronExpressionForEveryHour());
				_constants.IsInitializedRecurringCapitalLossCheckJob = true;
			}
		}

		public void InitializeRecurringRabbitMQMessageSenderJob()
		{
			if (!_constants.IsInitializedRecurringRabbitMQMessageSenderJob)
			{
				RecurringJob.AddOrUpdate("recurringRabbitMQMessageSender", () => _jobService.RecurringRabbitMQMessageSenderJob(), _dateTimeService.GetCronExpressionForEveryTenMinutes());
			}
		}

	}
}