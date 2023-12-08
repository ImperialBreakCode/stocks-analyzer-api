using API.Settlement.Domain.DTOs.Response.AvailabilityDTOs;
using API.Settlement.Domain.Interfaces.DateTimeInterfaces;
using API.Settlement.Domain.Interfaces.HangfireInterfaces;
using API.Settlement.Domain.Interfaces.HelpersInterfaces;
using Hangfire;

namespace API.Settlement.Application.Services.HangfireServices
{

	public class HangfireService : IHangfireService
	{
		private readonly IDateTimeHelper _dateTimeHelper;
		private readonly IHangfireJobService _hangfireJobService;
		private readonly IConstantsHelperWrapper _constants;

		public HangfireService(IDateTimeHelper dateTimeService,
							   IHangfireJobService hangfireJobService,
							   IConstantsHelperWrapper constants)
		{
			_dateTimeHelper = dateTimeService;
			_hangfireJobService = hangfireJobService;
			_constants = constants;
		}

		public void ScheduleStockProcessingJob(AvailabilityResponseDTO availabilityResponseDTO)
		{
			BackgroundJob.Schedule(() => _hangfireJobService.ProcessNextDayAccountTransaction(availabilityResponseDTO), _dateTimeHelper.GetTimeSpanUntilNextDayAtMinutePastMidnight());
		}

		public void InitializeRecurringFailedTransactionsJob()
		{
			if (!_constants.JobInitializationFlags.IsInitializedRecurringFailedTransactionsJob)
			{
				RecurringJob.AddOrUpdate("recurringFailedTransactionsJob", () => _hangfireJobService.RecurringFailedTransactionsJob(), _dateTimeHelper.GetCronExpressionForEveryHour());
				_constants.JobInitializationFlags.IsInitializedRecurringFailedTransactionsJob = true;
			}
		}

		public void InitializeRecurringCapitalLossJobCheck()
		{
			if (!_constants.JobInitializationFlags.IsInitializedRecurringCapitalLossCheckJob)
			{
				RecurringJob.AddOrUpdate("recurringCapitalLossJobCheck", () => _hangfireJobService.RecurringCapitalCheckJob(), _dateTimeHelper.GetCronExpressionForEveryHour());
				_constants.JobInitializationFlags.IsInitializedRecurringCapitalLossCheckJob = true;
			}
		}

		public void InitializeRecurringRabbitMQMessageSenderJob()
		{
			if (!_constants.JobInitializationFlags.IsInitializedRecurringRabbitMQMessageSenderJob)
			{
				RecurringJob.AddOrUpdate("recurringRabbitMQMessageSender", () => _hangfireJobService.RecurringRabbitMQMessageSenderJob(), _dateTimeHelper.GetCronExpressionForEveryTenMinutes());
			}
		}

	}
}