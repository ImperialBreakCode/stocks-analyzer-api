using API.Settlement.Domain.Interfaces.HelpersInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Application.Helpers.ConstantHelpers
{
	public class JobInitializationFlags : IJobInitializationFlags
	{
		private bool _isInitializedRecurringFailedTransactionsJob = false;
		private bool _isInitializedRecurringCapitalLossCheckJob = false;
		private bool _isInitializedRecurringRabbitMQMessageSenderJob = false;
		public bool IsInitializedRecurringFailedTransactionsJob
		{
			get
			{
				return _isInitializedRecurringFailedTransactionsJob;
			}
			set
			{
				_isInitializedRecurringFailedTransactionsJob = value;
			}
		}
		public bool IsInitializedRecurringCapitalLossCheckJob
		{
			get
			{
				return _isInitializedRecurringCapitalLossCheckJob;
			}
			set
			{
				_isInitializedRecurringCapitalLossCheckJob = value;
			}
		}
		public bool IsInitializedRecurringRabbitMQMessageSenderJob
		{
			get
			{
				return _isInitializedRecurringRabbitMQMessageSenderJob;
			}
			set
			{
				_isInitializedRecurringRabbitMQMessageSenderJob = value;
			}
		}

	}
}
