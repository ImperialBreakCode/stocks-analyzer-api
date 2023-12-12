using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Settlement.Domain.Interfaces.HelpersInterfaces
{
	public interface IJobInitializationFlags
	{
		public bool IsInitializedRecurringFailedTransactionsJob { get; set; }
		public bool IsInitializedRecurringCapitalLossCheckJob { get; set; }
		public bool IsInitializedRecurringRabbitMQMessageSenderJob { get; set; }
	}
}
